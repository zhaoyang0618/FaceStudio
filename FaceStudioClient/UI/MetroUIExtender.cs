using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FaceStudioClient.UI
{
    public static class MetroUIExtender
    {
        static MetroDialogSettings dlgsetting = new MetroDialogSettings();
        static MetroDialogSettings confirmSetting = new MetroDialogSettings();
        static ProgressDialogController currentController = null;
        static object controllerLock = new object();

        static MetroUIExtender()
        {
            dlgsetting.AffirmativeButtonText = "确 定";
            dlgsetting.NegativeButtonText = "取 消";
            confirmSetting.AffirmativeButtonText = "是(Yes)";
            confirmSetting.NegativeButtonText = "否(No)";
        }

        public static Task<MessageDialogResult> Alert(string msg, string title = "提示信息")
        {
            var win = (Application.Current.MainWindow as MetroWindow);
            return win.ShowMessageAsync(title, msg, MessageDialogStyle.Affirmative, dlgsetting);
        }

        public static async void SyncAlert(string msg, string title = "提示信息")
        {
            var win = (Application.Current.MainWindow as MetroWindow);
            await win.ShowMessageAsync(title, msg, MessageDialogStyle.Affirmative, dlgsetting);
        }

        public static Task<MessageDialogResult> Confirm(string msg, string title = "提示信息")
        {
            var win = (Application.Current.MainWindow as MetroWindow);
            return win.ShowMessageAsync(title, msg, MessageDialogStyle.AffirmativeAndNegative, dlgsetting);
        }

        public static async void SyncConfirm(string msg, string title = "提示信息")
        {
            var win = (Application.Current.MainWindow as MetroWindow);
            await win.ShowMessageAsync(title, msg, MessageDialogStyle.AffirmativeAndNegative, dlgsetting);
        }

        public static Task OpenDialog(CustomDialog dlg)
        {

            var win = (Application.Current.MainWindow as MetroWindow);
            return win.ShowMetroDialogAsync(dlg);
        }

        public static Task CloseDialog(CustomDialog dlg)
        {

            var win = (Application.Current.MainWindow as MetroWindow);
            return win.HideMetroDialogAsync(dlg);
        }

        public static async void SyncOpenDialog(CustomDialog dlg)
        {

            var win = (Application.Current.MainWindow as MetroWindow);
            await win.ShowMetroDialogAsync(dlg);
        }

        public static async void SyncCloseDialog(CustomDialog dlg)
        {

            var win = (Application.Current.MainWindow as MetroWindow);
            await win.HideMetroDialogAsync(dlg);
        }

        public static Task<MessageDialogResult> Alert(this UserControl control, string msg, string title = "提示信息")
        {
            var win = (Application.Current.MainWindow as MetroWindow);
            return win.ShowMessageAsync(title, msg);
        }

        public static Task<MessageDialogResult> Confirm(this UserControl control, string msg, string title = "提示信息")
        {
            var win = (Application.Current.MainWindow as MetroWindow);
            return win.ShowMessageAsync(title, msg, MessageDialogStyle.AffirmativeAndNegative);
        }

        public static Task<ProgressDialogController> Progress(string title, string msg, bool isCancelable = false)
        {
            var win = (Application.Current.MainWindow as MetroWindow);
            return win.ShowProgressAsync(title, msg, isCancelable, dlgsetting);
        }

        public static void SetController(ProgressDialogController c)
        {
            lock (controllerLock)
            {
                currentController = c;
            }
        }

        public static void CloseController()
        {
            lock (controllerLock)
            {
                if (currentController != null)
                {
                    currentController.CloseAsync();
                }
            }
        }

        public static void DoInController(Action<ProgressDialogController> action, Action action2 = null)
        {
            lock (controllerLock)
            {
                if (null != currentController)
                {
                    if (null != action)
                    {
                        action(currentController);
                    }
                }
                else
                {
                    if (action2 != null)
                    {
                        action2();
                    }
                }
            }
        }

        public static async void Progress(string title, string msg, bool isCancelable, Action<ProgressDialogController> configAction, Action<ProgressDialogController> doAction)
        {
            ProgressDialogController controller = null;
            lock (controllerLock)
            {
                if (null != currentController)
                {
                    controller = currentController;
                }
            }

            if (null == currentController)
            {
                var win = (Application.Current.MainWindow as MetroWindow);
                controller = await win.ShowProgressAsync(title, msg, isCancelable, dlgsetting);
                SetController(controller);
            }

            try
            {
                controller.SetCancelable(false);
                if (null != configAction)
                {
                    configAction(controller);
                }
                else
                {
                    //默认配置
                    controller.Canceled += (sender, e) => {
                        controller.CloseAsync();
                    };
                }
                //这个最后配置
                controller.Closed += (sender, e) => {
                    SetController(null);
                };

                System.Threading.Thread thread =
                    new System.Threading.Thread(new System.Threading.ParameterizedThreadStart((obj) => {
                        var c = obj as ProgressDialogController;
                        if (null != c)
                        {
                            doAction(c);
                        }
                        else
                        {
                            doAction(controller);
                        }
                    }));
                thread.IsBackground = true;
                thread.Start(controller);
            }
            catch (Exception exp)
            {
                if (isCancelable)
                {
                    controller.SetCancelable(true);
                    controller.SetTitle("发生异常");
                    controller.SetMessage(exp.Message);
                }
                else
                {
                    //直接关闭
                    await controller.CloseAsync();
                }
            }
        }

        public static void DefaultControllerConfig(ProgressDialogController controller)
        {
            controller.SetCancelable(false);
            controller.Canceled += (sender, e) => {
                controller.CloseAsync();
            };
            controller.Closed += (sender, e) => { };
        }
    }
}

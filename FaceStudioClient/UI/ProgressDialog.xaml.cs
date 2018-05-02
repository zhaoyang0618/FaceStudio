﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FaceStudioClient.UI
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog : UserControl
    {
        public event Action OnClose;

        public ProgressDialog()
        {
            InitializeComponent();
            InitUI();
            BindEvents();
        }

        #region 辅助函数
        void InitUI()
        {

        }

        void BindEvents()
        {
            btnClose.Click += (sender, e) => {
                if (null != OnClose)
                    OnClose();
            };
        }
        #endregion
    }
}
using Face.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Model
{
    class CameraUI : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public Camera Camera
        {
            get;set;
        }

        public bool IsActive
        {
            get;set;
        }

        public bool IsChecked
        {
            get;set;
        }

        public bool HasLogo
        {
            get
            {
                return null != this.Camera && this.Camera.Logo != null;
            }
        }

        public bool NoLogo
        {
            get
            {
                return !HasLogo;
            }
        }

        public string LogoURL
        {
            get
            {
                if (null == this.Camera || this.Camera.PhotoImageID == null)
                    return "http://localhost:8888//Images/RichVision.jpg";
                return Service.PhotoImageService.GetImageFileURL(this.Camera.PhotoImageID.Value);
            }
        }

        public void ChangeLogo(PhotoImage logo)
        {
            if (null == this.Camera || logo == null)
                return;
            this.Camera.Logo = logo;
            this.Camera.PhotoImageID = logo.ID;
            OnPropertyChanged("HasLogo");
            OnPropertyChanged("NoLogo");
            OnPropertyChanged("ChangeLogo");
        }
    }
}

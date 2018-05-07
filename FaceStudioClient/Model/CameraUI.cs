using Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudioClient.Model
{
    class CameraUI
    {
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
                return null;
            }
        }

        public void ChangeLogo(PhotoImage logo)
        {
            if (null == this.Camera || logo == null)
                return;
            this.Camera.Logo = logo;
            this.Camera.PhotoImageID = logo.ID;

        }
    }
}

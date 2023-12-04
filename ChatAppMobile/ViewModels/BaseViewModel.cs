using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.ViewModels
{
    public class BaseViewModel : BaseNotify
    {
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }


        private bool isHasError;

        public bool IsHasError
        {
            get { return isHasError; }
            set { SetProperty(ref isHasError, value); }
        }


        private string? error;

        public string? Error
        {
            get { return error; }
            set
            {
                SetProperty(ref error, value);
                IsHasError= string.IsNullOrEmpty(value) ? false : true;
            }
        }


    }
}

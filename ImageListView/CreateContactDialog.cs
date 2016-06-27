using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WebRequestTutorial
{
    public class CreateContactEventArgs : EventArgs
    {
        public string Name { get; set; }
        public string Birthday { get; set; }

        public CreateContactEventArgs(string name, string birthday)
        {
            Name = name;
            Birthday = birthday;
        }
    }

    class CreateContactDialog : DialogFragment
    {
        private Button mButtonCreateContact;
        private EditText txtName;
        private DatePicker txtDatePicker;

        public event EventHandler<CreateContactEventArgs> OnCreateContact;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_sign_up, container, false);
            mButtonCreateContact = view.FindViewById<Button>(Resource.Id.btnCreateContact);

            txtName = view.FindViewById<EditText>(Resource.Id.txtName);
            txtDatePicker = view.FindViewById<DatePicker>(Resource.Id.datePicker);

            mButtonCreateContact.Click += mButtonCreateContact_Click;

            return view;
           
        }

        void mButtonCreateContact_Click(object sender, EventArgs e)
        {
           if (OnCreateContact != null)
           {
                //Broadcast event
                OnCreateContact.Invoke(this, new CreateContactEventArgs(txtName.Text, txtDatePicker.DateTime.ToLongDateString()));              
           }

           this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation;
        }
    }
}
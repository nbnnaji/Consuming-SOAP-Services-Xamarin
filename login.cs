using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Web.Services;//newly added
using Mono.Data;
using MyEvents.EventsService; // Using directive for the webservice. The web reference name is EventsService

namespace MyEvents
{
    [Activity(Label = "Log In", MainLauncher = true)]
    public class LogIn : Activity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Screen in view is LoginForm xaml
            SetContentView(Resource.Layout.LoginForm);

            //Clear button activation
            var clearButton = FindViewById<Button>(Resource.Id.ClearButton);
            
            clearButton.Click += new EventHandler(clearButton_Click);

            //locate send button and trigger click event
            Button sendButton = FindViewById<Button>(Resource.Id.SsnSendButton);
            sendButton.Click += new EventHandler(sendButton_Click);

        }
        
        //click event of clear button
        void clearButton_Click(object sender, EventArgs e)
        {
            var textField = FindViewById<EditText>(Resource.Id.IDEditTextBox);
            textField.Text = " ";
         }

        //click event of send button
        void sendButton_Click(object sender, EventArgs e)
        {
            //locate send button and trigger click event
            EditText IDEditText = FindViewById<EditText>(Resource.Id.IDEditTextBox);

            //Asignment of text user enters in edittextbox to a string
            String ID = IDEditText.Text;

            //EventService is the name of the webservice
            var Login_WebService = new EventsService.EventService;
            
            //Webservice credentials
            EventsService.AuthenticateService webserviceAuthentication = new EventsService.AuthenticateService();
            webserviceAuthentication.serviceID = "Chuck";
            webserviceAuthentication.servicePASSWORD = "Nathaniel";

            //AuthenticateServiceValue is assigned webservice credentials
            Login_WebService.AuthenticateServiceValue = webserviceAuthentication;

            EventsService.REQUIREDPARAMS requiredLoginNumber = new EventsService.REQUIREDPARAMS();
            
            requiredLoginNumber.loginNum = 0;

            //Assignment of login number entered by user as a string
            requiredLoginNumber.loginNum = Decimal.Parse(SSNString);

            EventsService.CONTROLPARAMs controlParams = new EventsService.CONTROLPARAMs();
            EventsService.EVENTSPARAMs eventsParams = new EventsService.EVENTSPARAMs();
            EventsService.ResponseCONTROLPARAMs responseControlParams= new EventsService.ResponseCONTROLPARAMs();
            EventsService.ResponseEVENTSPARAMS responseEventsParams = new EventsService.ResponseEVENTSPARAMS();
            
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });

                Login_WebService.WebserviceMethod(requiredLoginNumber, controlParams, eventsParams, out responseControlParams, out responseEventsParams);

                //input validation
                if (ID == responseEventsParams.USER_ID_NUM.ToString())
                {
                    Intent eventActivityScreen = new Intent(this, typeof(eventsActivity));
                    senddatatoeventsActivityscreen.PutExtra("FirstName", responseEventsParams._USER_FIRST_NAME);//User firstname
                    senddatatoeventsActivityscreen.PutExtra("LastName", responseEventsParams._USER_LAST_NAME);//User lastname
                    
                    StartActivity(eventActivityScreen);
                                               
                  }
                else if (ID == null)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid entry!  " + responseControlParameter._PROVIDEDMESSAGE);
                }

            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("{0} No values entered.", ex);


            }
            catch(Exception ex)
            {
                throw new Exception("Error- server down", ex);
            }


            System.Diagnostics.Debug.WriteLine("*********************Testing********************");

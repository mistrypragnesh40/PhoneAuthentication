using Firebase;
using Firebase.Auth;
using PhoneAuthentication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAuthentication.Services
{
    public class AuthenticationService :PhoneAuthProvider.OnVerificationStateChangedCallbacks, IAuthenticationService
    {
        private TaskCompletionSource<bool> _verificationCodeCompletionSource;
        private string _verificationID;
        public Task<bool> AuthenticateMobile(string mobile)
        {
            _verificationCodeCompletionSource = new TaskCompletionSource<bool>();
            var authOption = PhoneAuthOptions.NewBuilder()
                 .SetPhoneNumber(mobile)
                 .SetTimeout((Java.Lang.Long)60L, Java.Util.Concurrent.TimeUnit.Seconds)
                 .SetActivity(Platform.CurrentActivity)
                 .SetCallbacks(this).Build();
            PhoneAuthProvider.VerifyPhoneNumber(authOption);
            return _verificationCodeCompletionSource.Task;

        }

        public override void OnCodeSent(string verificationID, PhoneAuthProvider.ForceResendingToken p1)
        {
            base.OnCodeSent(verificationID, p1);
            _verificationCodeCompletionSource.SetResult(true);
            _verificationID = verificationID;
        }

        public override void OnVerificationCompleted(PhoneAuthCredential p0)
        {
            System.Diagnostics.Debug.WriteLine("Verification completed");
        }

        public override void OnVerificationFailed(FirebaseException p0)
        {
            _verificationCodeCompletionSource.SetResult(false);
        }

        public async Task<bool> ValidateOTP(string code)
        {
            bool returnValue = false;
            if (!string.IsNullOrWhiteSpace(_verificationID))
            {
                var credential = PhoneAuthProvider.GetCredential(_verificationID, code);

                await FirebaseAuth.Instance.SignInWithCredentialAsync(credential).ContinueWith((authTask) =>
                {
                    if(authTask.IsFaulted || authTask.IsCanceled)
                    {
                        returnValue = false;
                        return;
                    }
                    returnValue = true;

                });
            }
            return returnValue;
        }
    }
}

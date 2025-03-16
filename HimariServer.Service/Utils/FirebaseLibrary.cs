using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Utils
{
    public class FirebaseLibrary
    {
        public static async Task<string> SendMessageFireBase(string title, string body, string token)
        {
            try
            {
                var message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    },
                    Token = token,
                    Android = new AndroidConfig()
                    {
                        Notification = new AndroidNotification()
                        {
                            Icon = "ic_notification",
                            Color = "#FF5733" 
                        }
                    }
                };

                var reponse = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return reponse;
            }
            catch
            {
                return "";
            }
        }

        public static async Task<bool> SendRangeMessageFireBase(string title, string body, List<string> tokens)
        {
            var message = new MulticastMessage()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                    ImageUrl = "https://firebasestorage.googleapis.com/v0/b/thelavenstore-fe036.appspot.com/o/HimariLogo.jpg?alt=media&token=33c99a89-4a91-4c9f-9b81-2aea896ed569"
                },
                Tokens = tokens
            };

            var reponse = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
            return true;

        }

        public static async Task<string> SendMessagePaymentFireBase(string title, string body, string token)
        {
            try
            {
                var message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body,
                        ImageUrl = "https://firebasestorage.googleapis.com/v0/b/thelavenstore-fe036.appspot.com/o/HimariLogo.jpg?alt=media&token=33c99a89-4a91-4c9f-9b81-2aea896ed569"
                    },
                    Token = token
                };

                var reponse = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return reponse;
            }
            catch
            {
                return "";
            }
        }
    }
}

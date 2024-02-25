using Elliptic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile
{
    public class ECCShare
    {
        public static byte[] GetPublicKey(byte[] privateKey)
        {
            try
            {
                return Curve25519.GetPublicKey(privateKey);
               
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static unsafe byte[] GetPublicKey(byte[]* v)
        {
            return Curve25519.GetPublicKey(*v);
        }
    }
}

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.Services;

public class Cryptography
{

    
 
    private static string password="sdfjhsldjfk";
 
    public static byte[] Key;
    public static byte[] Vector;

    public static string ConvertToHex(string asciiString)
    {
        char[] values = asciiString.ToCharArray();
        string res = "";
        foreach (char letter in values)
        {
            // Get the integral value of the character.
            int value = Convert.ToInt32(letter);
            // Convert the decimal value to a hexadecimal value in string form.
            string hexOutput = String.Format("{0:X}", value);
            res += hexOutput;
        }
        return res.ToUpper();
    }
    private static string ConvertToAscii(string asciiString)
    {
        string res = "";
        for (int i = 0; i < asciiString.Length; i+=2)
        {
            string hex = asciiString.Substring(i, 2);//ERROR INDEXOUTOFBOUNDS
            
            // Convert the number expressed in base-16 to an integer.
            int value = Convert.ToInt32(hex, 16);
            // Get the character corresponding to the integral value.
            string stringValue = Char.ConvertFromUtf32(value);
            char charValue = (char)value;
            res += stringValue;            
            
        }
        return  res;
    }

    public static void GenerateKey(string password)
    {
        SHA384Managed sha = new SHA384Managed();
        byte[] b = sha.ComputeHash(new ASCIIEncoding().GetBytes(password));
 
        Key = new byte[32];
        Vector = new byte[16];
 
        Array.Copy(b, 0, Key, 0, 32);
        Array.Copy(b, 32, Vector, 0, 16);
    }

    /*public string Encrypt(string plainText, string password)
    {
        GenerateKey(password);
        return Encrypt(plainText);
    }*/
    [WebMethod]
    public static string Encriptar(string plainText)
    {
        if (Key == null)
        {
            GenerateKey(password);
        }
 
        byte[] data = new ASCIIEncoding().GetBytes(plainText);
 
        RijndaelManaged crypto = new RijndaelManaged();

        crypto.Padding = PaddingMode.ISO10126;

        ICryptoTransform encryptor = crypto.CreateEncryptor(Key, Vector);
 
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream crptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
 
        crptoStream.Write(data, 0, data.Length);
        crptoStream.FlushFinalBlock();
 
        crptoStream.Close();
        memoryStream.Close();
 
        return ConvertToHex(Convert.ToBase64String(memoryStream.ToArray()));
    }
 
   
 
    public static string Decripta(string encryptedText)
    {
        if (Key == null)
        {
            GenerateKey(password);
        }


        string valor = ConvertToAscii(encryptedText);
        byte[] cipher = Convert.FromBase64String(valor);
 
        RijndaelManaged crypto = new RijndaelManaged();
        crypto.Padding = PaddingMode.ISO10126;
        ICryptoTransform encryptor = crypto.CreateDecryptor(Key, Vector);
 
        MemoryStream memoryStream = new MemoryStream(cipher);
        CryptoStream crptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Read);
 
        byte[] data = new byte[cipher.Length];
        int dataLength = crptoStream.Read(data, 0, data.Length);
 
        memoryStream.Close();
        crptoStream.Close();
 
        return (new ASCIIEncoding()).GetString(data, 0, dataLength);
    }



    /*
    #region Fields

    private static byte[] key = { };
    private static byte[] IV = { 38, 55, 206, 48, 28, 64, 20, 16 };
    private static string stringKey = "!5663a#KN";

    #endregion

    #region Public Methods


    private void prueba()
    {
        System.Security.Cryptography.Rijndael sys = new RijndaelManaged();
        
        
        
        Dim sym As New Encryption.Symmetric(Encryption.Symmetric.Provider.Rijndael)
Dim key As New Encryption.Data("My Password")
Dim encryptedData As New Encryption.Data
encryptedData.Base64 = base64EncryptedString
Dim decryptedData As Encryption.Data
decryptedData = sym.Decrypt(encryptedData, key)
Console.WriteLine(decryptedData.ToString)
    }

    private static string Base64Encode(string str)
    {
        byte[] encbuff = Encoding.UTF8.GetBytes(str);
        return HttpServerUtility.UrlTokenEncode(encbuff);
    }
    private static string Base64Decode(string str)
    {
        byte[] decbuff = HttpServerUtility.UrlTokenDecode(str);
        return Encoding.UTF8.GetString(decbuff);
    }

    private static string Encrypt(string text)
    {
        try
        {
            key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] byteArray = Encoding.UTF8.GetBytes(text);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                des.CreateEncryptor(key, IV), CryptoStreamMode.Write);

            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(memoryStream.ToArray()).;
        }
        catch (Exception ex)
        {
            // Handle Exception Here
        }

        return string.Empty;
    }
    public static string Encriptar(string text)
    {
        string parametro = Encrypt(text);
        return HttpUtility.UrlEncode(parametro);
        /*
        StringWriter writer = new StringWriter();

        HttpUtility.UrlEncode(parametro, writer);

        return string.Format("{0}", writer.ToString());
        /*HttpServerUtility server = System.Web.HttpContext.Current.Server;

        string parametro= Encrypt(text);
        return parametro;
        //return server.UrlEncode(parametro);

    }

    public static string Decripta(string text)
    {
        
        string parametro =text;
        
        //string parametro = Base64Decode(text);

        return Decrypt(parametro);

    }

    private static string Decrypt(string text)
    {

        try
        {
            key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] byteArray = Convert.FromBase64String(text);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                des.CreateDecryptor(key, IV), CryptoStreamMode.Write);

            cryptoStream.Write(byteArray, 0, byteArray.Length);
            cryptoStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        catch (Exception ex)
        {
            // Handle Exception Here
        }

        return string.Empty;
    }*/

    
}

using QRCoder;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ClothingStoreNew
{
    public partial class PaymentWindow : Window
    {
        private const string PaymentUrl = "https://github.com/aritakka/MagazinOdezhdi";

        public PaymentWindow()
        {
            InitializeComponent();
            GenerateQr();
        }

        private void GenerateQr()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(PaymentUrl, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

            byte[] qrBytes = qrCode.GetGraphic(20);

            QrImage.Source = LoadImage(qrBytes);
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            using (var ms = new MemoryStream(imageData))
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();
                img.Freeze();
                return img;
            }
        }
    }
}
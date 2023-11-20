using QRCoder;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Inventory_Context;

namespace Wpf_Inventarium
{
    public partial class QRCodeWindow : Window
    {
        public QRCodeWindow(Goods goods_for_qrcode)
        {
            InitializeComponent();
        
            var qrCodeData = MyQRCodeGenerator.GenerateQRCode(goods_for_qrcode);
            var qrCodeBitmap = new BitmapImage();

            using (var memoryStream = new MemoryStream(qrCodeData))
            {
                qrCodeBitmap.BeginInit();
                qrCodeBitmap.CacheOption = BitmapCacheOption.OnLoad;
                qrCodeBitmap.StreamSource = memoryStream;
                qrCodeBitmap.EndInit();
            }

            QRCodeImage.Source = qrCodeBitmap;
        }
    }

    public class MyQRCodeGenerator
    {
        public static byte[] GenerateQRCode(Goods goods)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(GetProductInformation(goods), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            using (var qrCodeImage = qrCode.GetGraphic(20))
            {
                using (var memoryStream = new MemoryStream())
                {
                    qrCodeImage.Save(memoryStream, ImageFormat.Png);
                    return memoryStream.ToArray();
                }
            }
        }

        private static string GetProductInformation(Goods goods)
        {
            return $"ProductId: {goods.goods_id}\n" +
                   $"ProductName: {goods.full_name}\n" +
                   $"Category: {goods.category}\n" +
                   $"Sub_category: {goods.subcategory}\n" +
                   $"Description: {goods.short_description}\n" +
                   $"Quantity: {goods.quantity}\n" +
                   $"Price: {goods.price}\n" +
                   $"WarehouseIdRef: {goods.warehouse_id_ref}";
        }
    }
}
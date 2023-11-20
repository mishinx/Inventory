using QRCoder;
using System.Drawing;
using DB;
using Inventory_Context;
using System.Drawing.Imaging;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic
{
    public class GoodsService
    {
        private readonly GoodsRepository _goodsRepository;

        public GoodsService(GoodsRepository goodsRepository)
        {
            _goodsRepository = goodsRepository;
        }

        public void CreateGoods(Goods goods)
        {
            try
            {
                _goodsRepository.Create(goods);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при створенні продукту: " + ex.Message);
            }
        }

        public List<Goods> GetAllGoodsForAdministrator(int adminId)
        {
            return _goodsRepository.GetAllGoodsForAdministrator(adminId);
        }

        public List<Goods> GetAllGoodsForOperator(int operatorId)
        {
            return _goodsRepository.GetAllGoodsForOperator(operatorId);
        }

        public bool UpdateGoods(Goods updatedGoods)
        {
            try
            {
                return _goodsRepository.Update(updatedGoods);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при оновленні продукту: " + ex.Message);
                return false;
            }
        }

        public bool DeleteGoods(int goodsId)
        {
            try
            {
                return _goodsRepository.Delete(goodsId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при видаленні продукту: " + ex.Message);
                return false;
            }
        }

        public List<Goods> GetFilteredGoods(string searchTerm)
        {
            return _goodsRepository.GetFilteredGoods(searchTerm);
        }

        public List<string> GetCategoriesForAdministrator(int adminId)
        {
            List<string> categories = _goodsRepository.GetCategoriesForAdministrator(adminId);

            return categories;
        }

        public List<string> GetSubCategoriesForAdministrator(int adminId, string category)
        {
            List<string> sub_categories = _goodsRepository.GetSubcategoriesForAdministrator(adminId, category);

            return sub_categories;
        }
    }
    public class ImageConverter
    {
        public static byte[] ConvertImageToByteArray(string imagePath, ImageFormat format)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                using (Image image = Image.FromFile(imagePath))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, format);
                        return ms.ToArray();
                    }
                }
            }

            return null;
        }
    }
}
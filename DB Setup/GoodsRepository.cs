﻿using Inventory_Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DB
{
    public class GoodsRepository
    {
        private readonly InventoryContext _context = new InventoryContext();
        ILogger _logger = LoggerManager.Instance.Logger;

        public virtual void Create(Goods new_goods)
        {
            _logger.Information("Додано товар");
            _context.goods.Add(new_goods);
            _context.SaveChanges();
        }

        public virtual List<Goods> GetAllGoodsForAdministrator(int adminId)
        {
            var goodsForAdministrator = _context.goods
            .Where(g => _context.warehouses
            .Where(w => w.admin_id_ref == adminId)
            .Select(w => w.warehouse_id)
            .Contains(g.warehouse_id_ref))
            .ToList();

            return goodsForAdministrator;
        }

        public virtual List<Goods> GetAllGoodsForOperator(int operatorId)
        {
            var operatorRecord = _context.operators.FirstOrDefault(o => o.operator_id == operatorId);

            if (operatorRecord != null)
            {
                var warehouseId = operatorRecord.warehouse_id_ref;

                var goodsForOperator = _context.goods
                    .Where(g => g.warehouse_id_ref == warehouseId)
                    .ToList();

                return goodsForOperator;
            }

            return new List<Goods>();
        }

        public virtual bool Update(Goods updatedGoods)
        {
            _logger.Information("Оновлено товар" + updatedGoods.goods_id.ToString());
            var goods_to_update = _context.goods.FirstOrDefault(g => g.goods_id == updatedGoods.goods_id);
            if (goods_to_update != null)
            {
                try
                {
                    _context.Entry(goods_to_update).State = EntityState.Detached;
                    _context.Entry(updatedGoods).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Помилка при оновленні продукту.", ex);
                }
            }

            return false;
        }

        public virtual bool Delete(int goodsId)
        {
            _logger.Information("Видалено товар" + goodsId.ToString());
            var goods_to_delete = _context.goods.FirstOrDefault(g => g.goods_id == goodsId);
            if (goods_to_delete != null)
            {
                try
                {
                    _context.goods.Remove(goods_to_delete);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Помилка при видаленні продукту.", ex);
                }
            }

            return false;
        }

        public virtual List<string> GetCategoriesForAdministrator(int adminId)
        {
            var categories = _context.goods
            .Where(g => _context.warehouses
                .Where(w => w.admin_id_ref == adminId)
                .Select(w => w.warehouse_id)
                .Contains(g.warehouse_id_ref))
            .Select(g => g.category)
            .Distinct()
            .ToList();

            return categories;
        }

        public virtual List<Goods> GetFilteredGoods(string searchTerm)
        {
            _logger.Information("Пошук товару за запитом - " + searchTerm);
            return _context.goods.Where(g => g.full_name.Contains(searchTerm)).ToList();
        }

        public virtual List<string> GetSubcategoriesForAdministrator(int adminId, string category)
        {
            var subcategories = _context.goods
                .Where(g => _context.warehouses
                    .Where(w => w.admin_id_ref == adminId)
                    .Select(w => w.warehouse_id)
                    .Contains(g.warehouse_id_ref))
                .Where(g => g.category == category)
                .Select(g => g.subcategory)
                .Distinct()
                .ToList();

            return subcategories;
        }
    }
}

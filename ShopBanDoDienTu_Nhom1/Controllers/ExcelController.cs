using OfficeOpenXml;
using ShopBanDoDienTu_Nhom1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDoDienTu_Nhom1.Controllers
{
    public class ExcelController : Controller
    {
        private MyDBContext db = new MyDBContext();

        // Action để export hóa đơn dưới dạng Excel
        public ActionResult ExportOrderToExcel(long orderDetailId)
        {
            // Lấy thông tin của chi tiết đơn hàng từ database
            OrderDetail orderDetail = db.OrderDetails.FirstOrDefault(od => od.OrderDetailId == orderDetailId);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }

            // Lấy thông tin của hóa đơn từ chi tiết đơn hàng
            Order order = db.Orders.FirstOrDefault(o => o.OrderId == orderDetail.OrderId);
            if (order == null)
            {
                return HttpNotFound();
            }

            // Tạo một workbook mới
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Hóa đơn");

            // Tiêu đề của cột
            worksheet.Cells[1, 1].Value = "STT";
            worksheet.Cells[1, 2].Value = "Tên sản phẩm";
            worksheet.Cells[1, 3].Value = "Số lượng";
            worksheet.Cells[1, 4].Value = "Đơn giá";
            worksheet.Cells[1, 5].Value = "Thành tiền";

            // Dữ liệu hóa đơn từ chi tiết đơn hàng
            int row = 2;
            worksheet.Cells[row, 1].Value = 1; // Chỉ có một sản phẩm nên chỉ có một dòng
            worksheet.Cells[row, 2].Value = orderDetail.Product != null ? orderDetail.Product.ProductName : "Unknown"; // Cần sửa lại chỗ này
            worksheet.Cells[row, 3].Value = orderDetail.Quantity;
            worksheet.Cells[row, 4].Value = orderDetail.UnitPrice;
            worksheet.Cells[row, 5].Value = orderDetail.Quantity * orderDetail.UnitPrice;

            // Tính tổng tiền
            decimal totalPrice = orderDetail.Quantity * orderDetail.UnitPrice;
            worksheet.Cells[row + 1, 4].Value = "Tổng tiền:";
            worksheet.Cells[row + 1, 5].Value = totalPrice;

            // Lưu workbook vào một stream
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);

            // Đặt header cho response
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Order_" + orderDetailId + ".xlsx");

            // Ghi stream vào response
            Response.BinaryWrite(stream.ToArray());
            Response.Flush();
            stream.Close();
            Response.End();

            return new EmptyResult();
        }
    }
}
using HimariServer.Service.BusinessModels.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Utils
{
    public class EmailUtils
    {
        public static string WelcomeEmail(string fullName)
        {
            string emailContent = $@"<!DOCTYPE html>
<html lang=""vi"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Chào mừng đến với Himari</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap');
        
        body {{
            font-family: 'Poppins', Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f8f8f8;
            color: #333333;
            line-height: 1.6;
            -webkit-font-smoothing: antialiased;
        }}
        
        table {{
            border-spacing: 0;
            border-collapse: collapse;
            mso-table-lspace: 0pt;
            mso-table-rspace: 0pt;
        }}
        
        td {{
            padding: 0;
        }}
        
        img {{
            border: 0;
            height: auto;
            line-height: 100%;
            outline: none;
            text-decoration: none;
            -ms-interpolation-mode: bicubic;
        }}
        
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.05);
        }}
        
        .email-header {{
            background-color: #ffd1dc;
            padding: 20px;
            text-align: center;
        }}
        
        .logo {{
            max-width: 150px;
            height: auto;
            display: inline-block;
        }}
        
        .email-body {{
            padding: 30px;
            background-color: #fff;
        }}
        
        .greeting {{
            font-size: 24px;
            font-weight: 600;
            margin-bottom: 20px;
            color: #ff85a2;
            text-align: center;
        }}
        
        .message {{
            font-size: 16px;
            margin-bottom: 25px;
            color: #555555;
            text-align: justify;
            -webkit-hyphens: auto;
            -ms-hyphens: auto;
            hyphens: auto;
        }}
        
        .button-container {{
            text-align: center;
            margin: 25px 0;
        }}
        
        .cta-button {{
            display: inline-block;
            background-color: #ff85a2;
            color: white !important;
            text-decoration: none;
            padding: 12px 30px;
            border-radius: 25px;
            font-weight: 500;
            font-size: 16px;
            text-align: center;
            box-shadow: 0 4px 8px rgba(255, 133, 162, 0.3);
            -webkit-text-size-adjust: none;
            mso-hide: all;
        }}
        
        .benefits {{
            margin: 30px 0;
            border-top: 1px solid #f0f0f0;
            padding-top: 20px;
        }}
        
        .benefit-item {{
            display: table;
            width: 100%;
            margin-bottom: 15px;
            table-layout: fixed;
        }}
        
        .benefit-icon {{
            display: table-cell;
            vertical-align: middle;
            width: 40px;
        }}
        
        .benefit-icon-inner {{
            width: 30px;
            height: 30px;
            background-color: #ffe0e6;
            border-radius: 50%;
            text-align: center;
            line-height: 30px;
            color: #ff85a2;
            font-weight: bold;
            display: inline-block;
        }}
        
        .benefit-text {{
            display: table-cell;
            vertical-align: middle;
            padding-left: 10px;
        }}
        
        .email-footer {{
            background-color: #ffd1dc;
            padding: 20px;
            text-align: center;
            font-size: 14px;
            color: #666;
        }}
        
        .social-icons {{
            margin: 15px 0;
            font-size: 0;
        }}
        
        .social-icon {{
            display: inline-block;
            margin: 0 8px;
            width: 36px;
            height: 36px;
            background-color: #ff85a2;
            border-radius: 50%;
            text-align: center;
            line-height: 36px;
            color: white;
            text-decoration: none;
            font-size: 16px;
        }}
        
        .footer-info {{
            margin-top: 10px;
            line-height: 1.5;
        }}
        
        @media only screen and (max-width: 600px) {{
            .email-container {{
                width: 100% !important;
                border-radius: 0 !important;
            }}
            
            .email-body {{
                padding: 20px !important;
            }}
            
            .greeting {{
                font-size: 22px !important;
            }}
            
            .message {{
                font-size: 15px !important;
            }}
            
            .button-container {{
                margin: 20px 0 !important;
                text-align: center !important;
                width: 100% !important;
            }}
            
            .cta-button {{
                display: block !important;
                width: 80% !important;
                margin: 0 auto !important;
                padding: 12px 20px !important;
                font-size: 16px !important;
                text-align: center !important;
            }}
            
            .benefit-item {{
                margin-bottom: 15px !important;
            }}
            
            .social-icon {{
                width: 34px !important;
                height: 34px !important;
                line-height: 34px !important;
            }}
        }}
    </style>
</head>
<body>
    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"" width=""100%"" style=""max-width: 600px;"" class=""email-container"">
        <tr>
            <td class=""email-header"">
                <img src=""https://firebasestorage.googleapis.com/v0/b/thelavenstore-fe036.appspot.com/o/HimariLogo.jpg?alt=media&token=33c99a89-4a91-4c9f-9b81-2aea896ed569"" alt=""Himari Logo"" class=""logo"">
            </td>
        </tr>
        
        <tr>
            <td class=""email-body"">
                <div class=""greeting"">Xin chào {fullName}!</div>
                
                <p class=""message"">
                    Cảm ơn bạn đã tham gia vào gia đình Himari! Chúng tôi rất vui mừng được đồng hành cùng bạn trong hành trình làm đẹp này.
                    Tại Himari, chúng tôi cam kết mang đến những sản phẩm mỹ phẩm chất lượng cao giúp tôn lên vẻ đẹp tự nhiên và tự tin của bạn.
                </p>
                
                <div class=""button-container"">
                    <a href=""himari://"" class=""cta-button"">Tải ứng dụng Himari</a>
                </div>
                
                <div class=""benefits"">
                    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"">
                        <tr class=""benefit-item"">
                            <td class=""benefit-icon"" width=""40"">
                                <span class=""benefit-icon-inner"">✓</span>
                            </td>
                            <td class=""benefit-text"">
                                Mỹ phẩm chất lượng cao được sản xuất với sự tỉ mỉ
                            </td>
                        </tr>
                        
                        <tr class=""benefit-item"">
                            <td class=""benefit-icon"" width=""40"">
                                <span class=""benefit-icon-inner"">✓</span>
                            </td>
                            <td class=""benefit-text"">
                                AI Hỗ trợ tư vấn sản phẩm thông minh
                            </td>
                        </tr>
                        
                        <tr class=""benefit-item"">
                            <td class=""benefit-icon"" width=""40"">
                                <span class=""benefit-icon-inner"">✓</span>
                            </td>
                            <td class=""benefit-text"">
                                Hàng ngàn ưu đãi độc quyền dành cho riêng bạn
                            </td>
                        </tr>
                    </table>
                </div>
                
                <p class=""message"">
                    Chúng tôi đã tạo tài khoản cá nhân cho bạn, nơi bạn có thể theo dõi đơn hàng,
                    lưu sản phẩm yêu thích và tận hưởng trải nghiệm mua sắm thuận tiện trên ứng dụng Himari.
                </p>
            </td>
        </tr>
        
        <tr>
            <td class=""email-footer"">
                <div class=""footer-info"">
                    © 2025 Himari Cosmetics. Đã đăng ký bản quyền.
                </div>
            </td>
        </tr>
    </table>
</body>
</html>";
            return emailContent;
        }

        public static string OrderMail(OrderResponseModel model)
        {
            string emailContent = $@"<!DOCTYPE html>
<html lang=""vi"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Xác nhận đơn hàng từ Himari</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap');
        
        body {{
            font-family: 'Poppins', Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f8f8f8;
            color: #333333;
            line-height: 1.6;
            -webkit-font-smoothing: antialiased;
        }}
        
        table {{
            border-spacing: 0;
            border-collapse: collapse;
            mso-table-lspace: 0pt;
            mso-table-rspace: 0pt;
        }}
        
        td {{
            padding: 0;
        }}
        
        img {{
            border: 0;
            height: auto;
            line-height: 100%;
            outline: none;
            text-decoration: none;
            -ms-interpolation-mode: bicubic;
        }}
        
        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.05);
        }}
        
        .email-header {{
            background-color: #ffd1dc;
            padding: 20px;
            text-align: center;
        }}
        
        .logo {{
            max-width: 150px;
            height: auto;
            display: inline-block;
        }}
        
        .email-body {{
            padding: 30px;
            background-color: #fff;
        }}
        
        .greeting {{
            font-size: 24px;
            font-weight: 600;
            margin-bottom: 20px;
            color: #ff85a2;
            text-align: center;
        }}
        
        .message {{
            font-size: 16px;
            margin-bottom: 25px;
            color: #555555;
            text-align: justify;
            -webkit-hyphens: auto;
            -ms-hyphens: auto;
            hyphens: auto;
        }}
        
        .order-info {{
            margin: 20px 0;
            border: 1px solid #ffe0e6;
            border-radius: 8px;
            padding: 15px;
            background-color: #fff5f7;
        }}
        
        .order-header {{
            font-size: 18px;
            font-weight: 600;
            color: #ff85a2;
            margin-bottom: 15px;
            text-align: center;
        }}
        
        .order-details {{
            font-size: 15px;
            color: #666;
            margin-bottom: 5px;
        }}
        
        .status-success {{
            color: #28a745;
            font-weight: 600;
        }}
        
        .product-table {{
            width: 100%;
            margin: 20px 0;
            border-collapse: collapse;
            box-shadow: 0 2px 5px rgba(255, 133, 162, 0.1);
            border-radius: 8px;
            overflow: hidden;
        }}
        
        .product-table th {{
            background-color: #ffe0e6;
            color: #ff5a87;
            font-weight: 500;
            text-align: left;
            padding: 12px;
            font-size: 14px;
        }}
        
        .product-table td {{
            padding: 12px;
            border-bottom: 1px solid #f0f0f0;
            color: #555;
            font-size: 14px;
        }}
        
        .product-table tr:nth-child(even) {{
            background-color: #fff9fa;
        }}
        
        .product-table tr:last-child td {{
            border-bottom: none;
        }}
        
        .text-right {{
            text-align: right;
        }}
        
        .product-name {{
            font-weight: 500;
            color: #444;
        }}
        
        .order-total {{
            margin-top: 15px;
            font-weight: 600;
            font-size: 16px;
            text-align: right;
            color: #ff5a87;
            padding: 10px 0;
            border-top: 2px solid #ffe0e6;
        }}
        
        .address-block {{
            background-color: #fff5f7;
            border: 1px solid #ffe0e6;
            border-radius: 8px;
            padding: 15px;
            margin: 20px 0;
        }}
        
        .address-title {{
            font-weight: 600;
            color: #ff85a2;
            margin-bottom: 8px;
        }}
        
        .button-container {{
            text-align: center;
            margin: 25px 0;
        }}
        
        .cta-button {{
            display: inline-block;
            background-color: #ff85a2;
            color: white !important;
            text-decoration: none;
            padding: 12px 30px;
            border-radius: 25px;
            font-weight: 500;
            font-size: 16px;
            text-align: center;
            box-shadow: 0 4px 8px rgba(255, 133, 162, 0.3);
            -webkit-text-size-adjust: none;
            mso-hide: all;
        }}
        
        .email-footer {{
            background-color: #ffd1dc;
            padding: 20px;
            text-align: center;
            font-size: 14px;
            color: #666;
        }}
        
        .footer-info {{
            margin-top: 10px;
            line-height: 1.5;
        }}
        
        @media only screen and (max-width: 600px) {{
            .email-container {{
                width: 100% !important;
                border-radius: 0 !important;
            }}
            
            .email-body {{
                padding: 20px !important;
            }}
            
            .greeting {{
                font-size: 22px !important;
            }}
            
            .message {{
                font-size: 15px !important;
            }}
            
            .product-table th,
            .product-table td {{
                padding: 10px 8px !important;
                font-size: 13px !important;
            }}
            
            .button-container {{
                margin: 20px 0 !important;
            }}
            
            .cta-button {{
                display: block !important;
                width: 80% !important;
                margin: 0 auto !important;
                padding: 10px 20px !important;
                font-size: 15px !important;
            }}
        }}
    </style>
</head>
<body>
    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" align=""center"" width=""100%"" style=""max-width: 600px;"" class=""email-container"">
        <tr>
            <td class=""email-header"">
                <img src=""https://firebasestorage.googleapis.com/v0/b/thelavenstore-fe036.appspot.com/o/HimariLogo.jpg?alt=media&token=33c99a89-4a91-4c9f-9b81-2aea896ed569"" alt=""Himari Logo"" class=""logo"">
            </td>
        </tr>
        
        <tr>
            <td class=""email-body"">
                <div class=""greeting"">Thanh toán thành công!</div>
                
                <p class=""message"">
                    Xin chào quý khách, cảm ơn bạn đã mua sắm tại Himari! Đơn hàng của bạn đã được thanh toán thành công và đang được xử lý.
                </p>
                
                <div class=""order-info"">
                    <div class=""order-header"">Thông tin đơn hàng</div>
                    <div class=""order-details"">Mã đơn hàng: <strong>#{model.OrderCode}</strong></div>
                    <div class=""order-details"">Ngày đặt hàng: <strong>{model.CreatedDate:dd/MM/yyyy HH:mm}</strong></div>
                    <div class=""order-details"">Trạng thái thanh toán: <span class=""status-success"">Thanh toán thành công</span></div>
                </div>
                
                <div class=""address-block"">
                    <div class=""address-title"">Địa chỉ giao hàng:</div>
                    <div>{model.Address}</div>
                </div>
                
                <div class=""order-header"">Chi tiết sản phẩm</div>
                <table class=""product-table"" cellspacing=""0"" cellpadding=""0"">
                    <tr>
                        <th>Sản phẩm</th>
                        <th>Số lượng</th>
                        <th class=""text-right"">Đơn giá</th>
                        <th class=""text-right"">Thành tiền</th>
                    </tr>
                    {GetProductRows(model.OrderDetails)}
                </table>
                
                <div class=""order-total"">Tổng thanh toán: {model.OrderPrice.ToString("N0")} đ</div>
                
                <p class=""message"">
                    Chúng tôi sẽ gửi thông báo khi đơn hàng của bạn được gửi đi.
                    Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với dịch vụ khách hàng của chúng tôi.
                </p>
                
                <div class=""button-container"">
                    <a href=""himari://orders/{model.Id}"" class=""cta-button"">Theo dõi đơn hàng</a>
                </div>
            </td>
        </tr>
        
        <tr>
            <td class=""email-footer"">
                <div class=""footer-info"">
                    © 2025 Himari Cosmetics. Đã đăng ký bản quyền.
                </div>
            </td>
        </tr>
    </table>
</body>
</html>";
            return emailContent;
        }

        private static string GetProductRows(List<OrderDetailsModel> products)
        {
            var rows = new StringBuilder();
            
            foreach (var product in products)
            {
                int subtotal = product.Price * product.Quantity;
                rows.Append($@"
                    <tr>
                        <td class=""product-name"">{product.ProductName}</td>
                        <td>{product.Quantity}</td>
                        <td class=""text-right"">{product.Price.ToString("N0")} đ</td>
                        <td class=""text-right"">{subtotal.ToString("N0")} đ</td>
                    </tr>");
            }
            
            return rows.ToString();
        }
    }
}

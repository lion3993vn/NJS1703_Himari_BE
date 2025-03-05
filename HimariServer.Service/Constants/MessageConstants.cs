using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Constants
{
    public class MessageConstants
    {
        public const string GET_USER_BY_EMAIL_SUCCESS = "Get user by email successfully";
        public const string USER_NOT_EXIST = "User is not exist";

        public const string GET_LIST_CATEGORY_SUCCESS = "Get list category successfully";
        public const string GET_CATEGORY_SUCCESS = "Get category successfully";
        public const string CATEGORY_NOT_FOUND = "Category not found";
        public const string CATEGORY_PARENT_NOT_FOUND = "Category parent not found";
        public const string CATEGORY_UPDATE_SUCCESS = "Category updated successfully";
        public const string CATEGORY_DELETE_SUCCESS = "Category deleted successfully";
        public const string CATEGORY_DELETE_FAIL = "Failed to delete category";
        public const string CATEGORY_CREATE_SUCCESS = "Category created successfully";
        public const string GET_PARENT_CATEGORIES_SUCCESS = "Get parent categories successfully";
        public const string GET_SUB_CATEGORIES_SUCCESS = "Get sub categories successfully";
        public const string GET_SUB_CATEGORIES_BY_PARENT_SUCCESS = "Get sub categories by parent successfully";

        public const string GET_LIST_PRODUCT_SUCCESS = "Get list product successfully";
        public const string PRODUCT_FOUND = "Product founds";
        public const string PRODUCT_NOT_FOUND = "Product not found";
        public const string PRODUCT_DELETE_SUCCESS = "Product deleted successfully";
        public const string PRODUCT_UPDATE_SUCCESS = "Product updated successfully";
        public const string PRODUCT_CREATE_SUCCESS = "Product created successfully";

        public const string GET_LIST_BRAND_SUCCESS = "Get list brand successfully";
        public const string BRAND_FOUND = "Brand founds";
        public const string BRAND_NOT_FOUND = "Brand not found";
        public const string BRAND_DELETE_SUCCESS = "Brand deleted successfully";
        public const string BRAND_UPDATE_SUCCESS = "Brand updated successfully";
        public const string BRAND_CREATE_SUCCESS = "Brand created successfully";

        public const string GET_LIST_SYMPTOM_SUCCESS = "Get list symptom successfully";
        public const string SYMPTOM_FOUND = "Symptom founds";
        public const string SYMPTOM_NOT_FOUND = "Symptom not found";
        public const string SYMPTOM_DELETE_SUCCESS = "Symptom deleted successfully";
        public const string SYMPTOM_UPDATE_SUCCESS = "Symptom updated successfully";
        public const string SYMPTOM_CREATE_SUCCESS = "Symptom created successfully";

        public const string GET_LIST_BODY_PART_SUCCESS = "Get list body part successfully";
        public const string BODY_PART_NOT_FOUND = "Body Part not found";
        public const string GET_BODY_PART_SUCCESS = "Get body part successfully";
        public const string BODY_PART_DELETE_SUCCESS = "Body part deleted successfully";
        public const string ADD_BODY_PART_SUCCESS = "Add body part successfully";
        public const string UPDATE_BODY_PART_SUCCESS = "Update body part successfully";

        public const string GET_LIST_BLOG_SUCCESS = "Get list blog successfully";
        public const string BLOG_FOUND = "Blog not founds";
        public const string BLOG_NOT_FOUND = "Blog not found";
        public const string BLOG_DELETE_SUCCESS = "Blog deleted successfully";
        public const string BLOG_UPDATE_SUCCESS = "Blog updated successfully";
        public const string BLOG_CREATE_SUCCESS = "Blog create successfully";
        public const string BLOG_REQUIRE_DATA = "Blog is require data";

        public const string GET_LIST_BLOG_CATEGORY_SUCCESS = "Get list blog category successfully";
        public const string BLOG_CATEGORY_FOUND = "Blog category not founds";
        public const string BLOG_CATEGORY_NOT_FOUND = "Blog category not found";
        public const string BLOG_CATEGORY_DELETE_SUCCESS = "Blog category deleted successfully";
        public const string BLOG_CATEGORY_UPDATE_SUCCESS = "Blog category updated successfully";
        public const string BLOG_CATEGORY_CREATE_SUCCESS = "Blog category create successfully";
        public const string BLOG_CATEROGY_REQUIRE_DATA = "Blog category is require data";

        public const string TOKEN_NOT_VALID = "Token not valid";
        public const string LOGIN_SUCCESS_MESSAGE = "Login successfully";
        public const string LOGIN_GOOGLE_SUCCESS_MESSAGE = "Login with google successfully";
        public const string TOKEN_REFRESH_SUCCESS_MESSAGE = "Token refresh successfully";
        public const string USER_HAS_BEEN_DELETE = "User has been deleted";

        public const string DEVICE_TOKEN_EXIST = "Device token exist";
        public const string DEVICE_TOKEN_NOT_EXIST = "Device token not exist";
        public const string DEVICE_TOKEN_ADD_SUCCESS = "Add device token for user successfully";
        public const string DEVICE_TOKEN_DELETE_SUCCESS = "Device token delete successfully";
        public const string USER_DEVICE_NOT_FOUND = "User device not found";

        public const string NOTI_NOT_EXIST = "Notification not exist";
        public const string USER_NOTI_NOT_EXIST = "User notification not exist";
        public const string GET_NOTI_SUCCESS = "Get notification successfully";
        public const string PUSH_NOTI_USER_SUCCESS = "Push notification to user successfully";
        public const string GET_LIST_NOTI_SUCCESS = "Get list notification successfully";
        public const string ENUM_NOTI_NOT_VALID = "Notification not valid";
        public const string COUNT_UNREAD_NOTI_SUCCESS = "Count unread notification success";
        public const string MARK_NOTI_AS_READ_SUCCESS = "Mark notification as read success";
        public const string MARK_ALL_NOTI_AS_READ_SUCCESS = "Mark all notification as read success";
        public const string NO_NOTI_MARK_AS_READ = "No notification mark as read";

        public const string PART_SYMPTOM_CREATE_SUCCESS = "Part symptom create successfully";
        public const string PART_SYMPTOM_NOT_FOUND = "Part symptom not exist";
        public const string PART_SYMPTOM_FOUND = "Get part symptom successfully";
        public const string PART_SYMPTOM_UPDATE_SUCCESS = "Update part symptom successfully";
        public const string PART_SYMPTOM_DELETE_SUCCESS = "Delete part symptom successfully";

        public const string GET_CHAT_MESSAGES_SUCCESS = "Get chat messages successfully";

        public const string ORDER_ITEM_NOT_HAVE = "Order item must be at least 1 item";
        public const string ORDER_ITEM_NOT_FOUND = "Item id {id} not found";
        public const string INSUFFICIENT_STOCK_QUANTITY = "Item {name} is insufficient for this order";
        public const string PAYMENT_DESCRIPTION = "Đơn hàng ";
        public const string ORDER_CREATE_SUCCESS = "Create order successfully";

        public const string UPLOAD_FILE_SUCCESS = "Upload file to firebase successfully";
        public const string NO_FILE_UPLOAD = "No file to upload firebase";
        public const string IMAGE_EXTENSION_NOT_SUPPORT = "Image extension not support";

        public const string PRODUCT_SYMPTOM_NOT_FOUND = "Product symptom not found";
        public const string PRODUCT_SYMPTOM_FOUND = "Product symptom found successfully";
        public const string PRODUCT_SYMPTOM_CREATE_SUCCESS = "Product symptom created successfully";
        public const string PRODUCT_SYMPTOM_DELETE_SUCCESS = "Product symptom delete successfully";
        public const string GET_LIST_PRODUCT_SYMPTOM_SUCCESS = "Get list product symptom successfully";
        public const string PRODUCT_SYMPTOM_UPDATE_SUCCESS = "Update product symptom successfully";

    }
}

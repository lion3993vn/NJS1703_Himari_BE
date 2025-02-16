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
        public const string ACCOUNT_NOT_EXIST = "Account is not exist";

        public const string GET_LIST_CATEGORY_SUCCESS = "Get list category successfully";
        public const string GET_CATEGORY_SUCCESS = "Get category successfully";
        public const string CATEGORY_NOT_FOUND = "Category not found";
        public const string CATEGORY_PARENT_NOT_FOUND = "Category parent not found";
        public const string CATEGORY_UPDATE_SUCCESS = "Category updated successfully";
        public const string CATEGORY_DELETE_SUCCESS = "Category deleted successfully";

        public const string GET_LIST_PRODUCT_SUCCESS = "Get list product successfully";
        public const string PRODUCT_FOUND = "Product not founds";
        public const string PRODUCT_NOT_FOUND = "Product not found";
        public const string PRODUCT_DELETE_SUCCESS = "Product deleted successfully";
        public const string PRODUCT_UPDATE_SUCCESS = "Product updated successfully";

        public const string TOKEN_NOT_VALID = "TokenNotValid";
        public const string LOGIN_SUCCESS_MESSAGE = "Login successfully";
        public const string LOGIN_GOOGLE_SUCCESS_MESSAGE = "Login with google successfully";
        public const string TOKEN_REFRESH_SUCCESS_MESSAGE = "Token refresh successfully";
    }
}

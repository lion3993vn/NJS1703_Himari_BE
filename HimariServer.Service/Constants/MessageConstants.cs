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
        public const string CATEGORY_CREATE_SUCCESS = "Category created successfully";

        public const string GET_LIST_PRODUCT_SUCCESS = "Get list product successfully";
        public const string PRODUCT_FOUND = "Product founds";
        public const string PRODUCT_NOT_FOUND = "Product not found";
        public const string PRODUCT_DELETE_SUCCESS = "Product deleted successfully";
        public const string PRODUCT_UPDATE_SUCCESS = "Product updated successfully";

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


        public const string TOKEN_NOT_VALID = "Token not valid";
        public const string LOGIN_SUCCESS_MESSAGE = "Login successfully";
        public const string LOGIN_GOOGLE_SUCCESS_MESSAGE = "Login with google successfully";
        public const string TOKEN_REFRESH_SUCCESS_MESSAGE = "Token refresh successfully";
        public const string USER_HAS_BEEN_DELETE = "User has been deleted";
    }
}

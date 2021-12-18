using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagement.utils {
    public class Validate {
        public static bool IsValidEmail(string email) {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match) {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e) {
                return false;
            }
            catch (ArgumentException e) {
                return false;
            }

            try {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException) {
                return false;
            }
        }
        public static Boolean isStoreValidate(BookStore bookStore) {
            if (bookStore == null)
                return false;

            String storeName = bookStore.Name;
            if (storeName.Length < 4) {
                MessageBox.Show("Tên nhà sách phải có tổi thiểu 4 kí tự", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            String storeAddress = bookStore.Address;
            if (storeAddress == null || storeAddress.Length == 0) {
                MessageBox.Show("Vui lòng nhập địa chỉ nhà sách", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            String storePhone = bookStore.Phone;
            if (storePhone != "" && storePhone.Length < 8) {
                MessageBox.Show("Số điện thoại phải có tổi thiểu 8 kí tự", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            String stroreEmail = bookStore.Email;
            if (stroreEmail != "" && !IsValidEmail(stroreEmail)) {
                MessageBox.Show("Email không hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}

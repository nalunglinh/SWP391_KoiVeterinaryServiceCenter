import React, { useEffect, useState } from "react";
import axios from "axios";
import Header from "../components/Header";
import Footer from "../components/Footer";

function EditProfile() {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [formErrors, setFormErrors] = useState({
    fullName: "",
    email: "",
    phone: "",
  });

  useEffect(() => {
    axios
      .get("http://localhost:5104/api/Account/profile", {
        withCredentials: true,
      })
      .then((response) => {
        setProfile(response.data);
      })
      .catch((error) => {
        console.error("There was an error fetching the profile data!", error);
        setError("Unable to fetch profile data.");
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  const validateEmail = (email) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  };

  const validatePhone = (phone) => {
    const phoneRegex = /^0\d{9}$/;
    return phoneRegex.test(phone);
  };

  const validateFullName = (fullName) => {
    return fullName.length > 4; // Kiểm tra xem độ dài của fullName có lớn hơn 4 không
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const fullName = e.target.fullName.value;
    const email = e.target.email.value;
    const phone = e.target.phone.value;
    const dob = e.target.dob.value;
    const userAddress = e.target.userAddress.value;

    let errors = {
      fullName: "",
      email: "",
      phone: "",
    };

    // Kiểm tra tính hợp lệ của fullName, email và phone
    if (!validateFullName(fullName)) {
      errors.fullName = "Full Name must be more than 4 characters.";
    }

    if (!validateEmail(email)) {
      errors.email = "Please enter a valid email address.";
    }

    if (!validatePhone(phone)) {
      errors.phone =
        "Phone number must start with 0 and contain exactly 10 digits.";
    }

    // Nếu có lỗi, cập nhật trạng thái và không tiếp tục
    if (errors.fullName || errors.email || errors.phone) {
      setFormErrors(errors);
      return;
    }

    // Nếu không có lỗi, tiếp tục cập nhật hồ sơ
    const updatedProfile = {
      fullName,
      email,
      phone,
      dob,
      userAddress,
    };

    try {
      const response = await axios.put(
        "http://localhost:5104/api/Account/edit-profile",
        updatedProfile,
        { withCredentials: true }
      );
      alert(response.data); // Hiển thị thông báo thành công
      window.location.href = "/profile"; // Giả sử bạn có đường dẫn /profile cho trang hồ sơ
    } catch (error) {
      console.error("There was an error updating the profile!", error);
      alert("Unable to update profile.");
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <>
      <Header />
      <div className="container mt-5">
        <div className="row justify-content-center">
          <div className="col-md-6">
            <div className="card shadow-lg border-0 rounded-lg">
              <div className="card-header bg-primary text-white text-center">
                <h3 className="mb-0">Edit Profile</h3>
              </div>
              <div className="card-body">
                <form onSubmit={handleSubmit}>
                  <div className="mb-3">
                    <label htmlFor="fullName" className="form-label">
                      Full Name
                    </label>
                    <input
                      type="text"
                      className={`form-control ${
                        formErrors.fullName ? "is-invalid" : ""
                      }`}
                      id="fullName"
                      defaultValue={profile.fullName}
                      required
                    />
                    {formErrors.fullName && (
                      <div className="invalid-feedback">
                        {formErrors.fullName}
                      </div>
                    )}
                  </div>
                  <div className="mb-3">
                    <label htmlFor="email" className="form-label">
                      Email
                    </label>
                    <input
                      type="email"
                      className={`form-control ${
                        formErrors.email ? "is-invalid" : ""
                      }`}
                      id="email"
                      defaultValue={profile.email}
                      required
                    />
                    {formErrors.email && (
                      <div className="invalid-feedback">{formErrors.email}</div>
                    )}
                  </div>
                  <div className="mb-3">
                    <label htmlFor="phone" className="form-label">
                      Phone
                    </label>
                    <input
                      type="text"
                      className={`form-control ${
                        formErrors.phone ? "is-invalid" : ""
                      }`}
                      id="phone"
                      defaultValue={profile.phone}
                    />
                    {formErrors.phone && (
                      <div className="invalid-feedback">{formErrors.phone}</div>
                    )}
                  </div>
                  <div className="mb-3">
                    <label htmlFor="dob" className="form-label">
                      Date of Birth
                    </label>
                    <input
                      type="date"
                      className="form-control"
                      id="dob"
                      defaultValue={profile.dob.split("T")[0]}
                    />
                  </div>
                  <div className="mb-3">
                    <label htmlFor="userAddress" className="form-label">
                      User Address
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      id="userAddress"
                      defaultValue={profile.userAddress}
                    />
                  </div>
                  <div className="text-center">
                    <button type="submit" className="btn btn-primary">
                      Save Changes
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </>
  );
}

export default EditProfile;

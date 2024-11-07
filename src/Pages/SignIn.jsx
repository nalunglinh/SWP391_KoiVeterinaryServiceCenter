import { message } from "antd";
import axios from "axios";
import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { ArrowLeftOutlined } from "@ant-design/icons"; // Thêm biểu tượng mũi tên quay lại

function SignIn() {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleEmailChange = (event) => {
    setEmail(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const handleLogin = async () => {
    if (!email || !password) {
      message.error("Please fill in both email and password.");
      return;
    }

    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!emailPattern.test(email)) {
      message.error("Please enter a valid email address.");
      return;
    }

    try {
      const response = await axios.post(
        "http://localhost:5104/api/account/login",
        {
          email: email,
          password: password,
        },
        { withCredentials: true }
      );
      console.log("re", response.data);

      const successMessage = "Login successful!";
      message.success(successMessage);
     
      // Lưu customerId vào localStorage
      const user = response.data;
      localStorage.setItem("CurrentUser", JSON.stringify(user));

      const redirectPath = sessionStorage.getItem("redirectPath") || "/"; 
      sessionStorage.removeItem("redirectPath");

      navigate(redirectPath); // Chuyển hướng về trang chủ sau khi đăng nhập thành công
    } catch (error) {
      if (error.response) {
        console.error("Login error response:", error.response);
        message.error("Login failed. Please check your credentials.");
      } else {
        console.error("Login error:", error);
        message.error("An unexpected error occurred. Please try again later.");
      }
    }
  };

  return (
    <div>
      <section className="vh-100" style={{ backgroundColor: "#110253" }}>
        <div className="container py-5 h-100">
          <div className="row d-flex justify-content-center align-items-center h-100">
            <div className="col col-xl-10">
              <div className="card" style={{ borderRadius: "1rem" }}>
                <div className="row g-0">
                  <div className="col-md-6 col-lg-5 d-none d-md-block">
                    <img
                      src="https://images.unsplash.com/photo-1618419125747-ee5a210c6ebe?q=80&w=1887&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                      alt="login form"
                      className="img-fluid"
                      style={{
                        borderRadius: "1rem 0 0 1rem",
                        objectFit: "cover", // Đảm bảo hình ảnh fill
                        height: "100%", // Đặt chiều cao là 100%
                      }}
                    />
                  </div>
                  <div className="col-md-6 col-lg-7 d-flex align-items-center">
                    <div className="card-body p-4 p-lg-5 text-black">
                      <div className="d-flex align-items-center mb-3 pb-1">
                        <ArrowLeftOutlined
                          onClick={() => navigate(-1)} // Chuyển hướng quay lại
                          style={{ cursor: "pointer", fontSize: "20px", color: "#ff6219", marginRight: "10px" }}
                        />
                        <span className="h1 fw-bold mb-0">You&apos;re back!</span>
                      </div>
                      <h5 className="fw-normal mb-3 pb-3" style={{ letterSpacing: 1 }}>
                        Please fill in the information below to log in.
                      </h5>
                      <form onSubmit={(e) => e.preventDefault()}>
                        <div className="form-outline mb-4">
                          <label className="form-label" htmlFor="email">
                            Email
                          </label>
                          <input
                            type="email"
                            id="email"
                            className="form-control form-control-lg"
                            placeholder="Enter your email"
                            value={email}
                            onChange={handleEmailChange}
                          />
                        </div>
                        <div className="form-outline mb-4">
                          <label className="form-label" htmlFor="password">
                            Password
                          </label>
                          <input
                            type="password"
                            id="password"
                            className="form-control form-control-lg"
                            placeholder="Enter your password"
                            value={password}
                            onChange={handlePasswordChange}
                          />
                        </div>
                        <div className="pt-1 mb-4">
                          <button
                            className="btn btn-dark btn-lg btn-block"
                            type="button"
                            onClick={handleLogin}
                          >
                            Login
                          </button>
                        </div>
                        <Link to="/ForgotPassword" style={{ color: "#393f81" }}>
                          Forgot Password?
                        </Link>
                        <p className="mb-5 pb-lg-2" style={{ color: "#393f81" }}>
                          Don&apos;t have an account?{" "}
                          <Link to="/Sign-Up" style={{ color: "#393f81" }}>
                            Sign up here
                          </Link>
                        </p>
                      </form>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}

export default SignIn;

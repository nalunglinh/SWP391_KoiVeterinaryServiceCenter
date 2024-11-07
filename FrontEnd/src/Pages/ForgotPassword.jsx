import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import { Input, Button, Card, Row, Col, message } from "antd";

function ForgotPassword() {
  const [email, setEmail] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "http://localhost:5104/api/Account/forgot-password",
        {
          email,
        }
      );
      message.success("Password reset email sent! Please check your inbox.");
      navigate("/ResetPassword"); // Corrected this line
    } catch (error) {
      message.error("Failed to send reset email. Please try again.");
    }
  };

  return (
    <div className="container">
      <Row justify="center">
        <Col xs={24} md={12}>
          <Card title="Forgot Password" className="shadow p-4">
            <p className="text-center">
              Enter your email to reset your password
            </p>
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label htmlFor="email" className="form-label">
                  Email Address
                </label>
                <Input
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  placeholder="name@example.com"
                  required
                />
              </div>
              <Button type="primary" htmlType="submit" className="w-100 mb-2">
                Submit
              </Button>
            </form>
            <Link to="/login">
              <Button type="link" className="w-100">
                Back to Login
              </Button>
            </Link>
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export default ForgotPassword;

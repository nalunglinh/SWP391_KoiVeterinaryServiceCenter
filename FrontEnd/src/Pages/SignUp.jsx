import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { Form, Input, Button, Checkbox, message } from 'antd';
import axios from 'axios';

function SignUp() {
  const nagative = useNavigate();
  const onFinish = (values) => {
    axios({
      method: 'post',
      url: 'http://localhost:5104/api/Account/register',
      data: values,
    })
      .then(() => {
        message.success('Registration successful!');
        nagative("/login")
      })
      .catch((error) => {
        message.error(`Registration failed: ${error}`);
      });
  };

  return (
    <div>
      <section className="vh-100" style={{ backgroundColor: "#0e009f" }}>
        <div className="container h-100">
          <div className="row d-flex justify-content-center align-items-center h-100">
            <div className="col-lg-12 col-xl-11">
              <div className="card text-black" style={{ borderRadius: 25 }}>
                <div className="card-body p-md-5">
                  <div className="row justify-content-center">
                    <div className="col-md-10 col-lg-6 col-xl-5 order-2 order-lg-1">
                      <p className="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Sign up</p>
                      <Form layout="vertical" onFinish={onFinish} className="mx-1 mx-md-4">
                        <Form.Item
                          name="fullName"
                          label="Your full Name"
                          rules={[{ required: true, message: 'Please input your name!' }]}
                        >
                          <Input />
                        </Form.Item>

                        <Form.Item
                          name="email"
                          label="Email"
                          rules={[
                            { required: true, type: 'email', message: 'Please input a valid email!' },
                          ]}
                        >
                          <Input />
                        </Form.Item>

                        <Form.Item
                          name="phone"
                          label="Phone"
                          rules={[
                            { required: true, message: 'Please input a valid phone!' },
                            {
                              pattern: /^\d{10,15}$/,
                              message: 'Phone number must be between 10 and 15 digits.',
                            },
                          ]}
                        >
                          <Input />
                        </Form.Item>


                        <Form.Item
                          name="password"
                          label="Password"
                          rules={[{ required: true, message: 'Please input your password!' }]}
                        >
                          <Input.Password />
                        </Form.Item>

                        <Form.Item
                          name="confirmPassword"
                          label="Confirm Password"
                          dependencies={['password']}
                          rules={[
                            { required: true, message: 'Please confirm your password!' },
                            ({ getFieldValue }) => ({
                              validator(_, value) {
                                if (!value || getFieldValue('password') === value) {
                                  return Promise.resolve();
                                }
                                return Promise.reject(new Error('The two passwords do not match!'));
                              },
                            }),
                          ]}
                        >
                          <Input.Password />
                        </Form.Item>

                        <Form.Item name="agreement" valuePropName="checked" className="form-check d-flex justify-content-center mb-5">

                          <Link to={`/login`}>Sign in here</Link>

                        </Form.Item>

                        <Form.Item className="d-flex justify-content-center mx-4 mb-3 mb-lg-4">
                          <Button type="primary" htmlType="submit" size="large">
                            Register
                          </Button>
                        </Form.Item>
                      </Form>
                    </div>
                    <div className="col-md-10 col-lg-6 col-xl-7 d-flex align-items-center order-1 order-lg-2">
                      <img
                        src="https://plus.unsplash.com/premium_photo-1681966826227-d008a1cfe9c7?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                        className="img-fluid"
                        alt="Sample"
                      />
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

export default SignUp;

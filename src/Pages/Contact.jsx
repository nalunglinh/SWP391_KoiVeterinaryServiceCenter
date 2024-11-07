import React from "react";
import Header from "../components/Header";
import Footer from "../components/Footer";
import { Link } from "react-router-dom";

function Contact() {
  return (
    <>
      <>
        <meta charSet="UTF-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>Contact Form</title>
        <Header></Header>
        <link
          href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css"
          rel="stylesheet"
        />
        <style
          dangerouslySetInnerHTML={{
            __html:
              "\n        .form-control:focus {\n            border-color: #FF6F61;\n            box-shadow: 0 0 0 0.2rem rgba(255, 111, 97, 0.25);\n        }\n        .form-text {\n            color: red;\n        }\n        .char-count {\n            font-size: 0.9rem;\n            color: gray;\n        }\n        button {\n            background-color: #FF6F61;\n            color: white;\n        }\n    ",
          }}
        />
        <div className="container mt-5">
          <div className="row justify-content-center">
            <div className="col-md-8">
              <form>
                <div className="row mb-3">
                  <div className="col">
                    <label htmlFor="firstName" className="form-label">
                      First name <span className="form-text">*</span>
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      id="firstName"
                      placeholder="8 to 32 characters"
                      required=""
                    />
                  </div>
                  <div className="col">
                    <label htmlFor="lastName" className="form-label">
                      Last name <span className="form-text">*</span>
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      id="lastName"
                      placeholder="8 to 32 characters"
                      required=""
                    />
                  </div>
                </div>
                <div className="mb-3">
                  <label htmlFor="email" className="form-label">
                    Email <span className="form-text">*</span>
                  </label>
                  <input
                    type="email"
                    className="form-control"
                    id="email"
                    placeholder="youremail@gmail.com"
                    required=""
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="phone" className="form-label">
                    Phone number
                  </label>
                  <input
                    type="tel"
                    className="form-control"
                    id="phone"
                    placeholder="If you would like us to call you back, please leave a contact number."
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="message" className="form-label">
                    Messages
                  </label>
                  <textarea
                    className="form-control"
                    id="message"
                    rows={4}
                    defaultValue={""}
                  />
                </div>
                <div className="text-center">
                  <Link to="/" className="btn btn-lg w-100">
                    Submit
                  </Link>
                </div>
              </form>
            </div>
          </div>
          <Footer></Footer>
        </div>
      </>
    </>
  );
}

export default Contact;

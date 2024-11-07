import React from "react";
import { Link, useNavigate } from "react-router-dom";

function Header() {
  const message = localStorage.getItem("CurrentUser");
  const navigate = useNavigate();

  const handleLogout = () => {
    // Gửi yêu cầu đăng xuất đến API (nếu cần)
    // axios.post('http://localhost:5104/api/Account/logout', {}, { withCredentials: true })
    //     .then(() => {
    localStorage.removeItem("CurrentUser");
    navigate("/"); // Chuyển hướng về trang chính
    window.location.reload(); // Làm mới trang
    //     })
    //     .catch(error => {
    //         console.error("Logout error:", error);
    //     });
  };

  return (
    <header className="text-black bg-light shadow-sm">
      <div className="container">
        <div className="d-flex flex-wrap align-items-center justify-content-between py-3">
          <a
            href="/"
            className="d-flex align-items-center col-md-3 mb-2 mb-md-0 text-dark text-decoration-none"
          >
            <svg
              className="bi me-2"
              width={40}
              height={32}
              role="img"
              aria-label="Logo"
            >
              <use xlinkHref="#logo" />
            </svg>
            <div className="logo">🐟</div>
            <span className="fs-4">KoiCare</span>
          </a>
          <ul className="nav col-12 col-md-auto mb-2 justify-content-center mb-md-0">
            <li>
              <Link
                to={`/services`}
                className="nav-link px-3 text-dark link-hover"
              >
                Service{" "}
              </Link>
            </li>
            <li>
              <Link
                to={`/StandardCare`}
                className="nav-link px-3 text-dark link-hover"
              >
                Standard Care
              </Link>
            </li>
            <li>
              <Link
                to={`/FAQPage`}
                className="nav-link px-3 text-dark link-hover"
              >
                FAQ
              </Link>
            </li>
            <li>
              <Link
                to={`/Contact`}
                className="nav-link px-3 text-dark link-hover"
              >
                Contact
              </Link>
            </li>
          </ul>
          {message ? (
            <div className="dropdown">
              <button
                className="btn nav-link dropdown-toggle text-dark name"
                type="button"
                id="userDropdown"
                data-bs-toggle="dropdown"
                aria-expanded="false"
                style={{
                  backgroundColor: "transparent",
                  color: "#343a40",
                  fontWeight: 500,
                  border: "none",
                  padding: "8px 12px",
                  borderRadius: "5px",
                  transition: "background-color 0.3s ease",
                }}
                onMouseOver={(e) => {
                  e.currentTarget.style.backgroundColor = "#f1f1f1";
                  e.currentTarget.style.color = "#007bff";
                }}
                onMouseOut={(e) => {
                  e.currentTarget.style.backgroundColor = "transparent";
                  e.currentTarget.style.color = "#343a40";
                }}
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="16"
                  height="16"
                  fill="currentColor"
                  className="bi bi-person-fill"
                  viewBox="0 0 16 16"
                >
                  <path d="M3 14s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1zm5-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6" />
                </svg>
              </button>

              <ul
                className="dropdown-menu dropdown-menu-end"
                aria-labelledby="userDropdown"
              >
                <li>
                  <Link className="dropdown-item" to="/Profile">
                    Profile
                  </Link>
                </li>
                <li>
                  <hr className="dropdown-divider" />
                </li>
                <Link className="dropdown-item" to="/History">
                  History
                </Link>
                <li>
                  <hr className="dropdown-divider" />
                </li>
                <li>
                  <button className="dropdown-item" onClick={handleLogout}>
                    Log out
                  </button>
                </li>
              </ul>
            </div>
          ) : (
            <div className="col-md-3 text-end">
              <Link
                to={`/login`}
                className="btn btn-outline-primary me-2 rounded-pill px-4"
              >
                Login
              </Link>
              <Link
                to={`/Sign-up`}
                className="btn btn-primary rounded-pill px-4"
              >
                Sign-up
              </Link>
            </div>
          )}
        </div>
      </div>
    </header>
  );
}

export default Header;

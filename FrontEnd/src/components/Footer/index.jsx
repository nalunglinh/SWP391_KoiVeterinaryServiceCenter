import React from 'react';

function Footer() {
    return (
        <div className="container">
            <footer className="row row-cols-1 row-cols-sm-2 row-cols-md-2 py-5 my-5 border-top">
                <div className="col mb-3">
                    <a
                        href="/"
                        className="d-flex align-items-center mb-3 link-dark text-decoration-none"
                    >
                        <svg className="bi me-2" width={40} height={32}>
                            <use xlinkHref="#bootstrap" />
                        </svg>
                    </a>
                    <p className="text-light">© 2024 Veterinary Hospital</p>
                </div>
                <div className="col mb-3">
                    <h5>Contact Us</h5>
                    <p className="text-light">nhatanhok555@gmail.com</p>
                    <p className="text-light">(84+) 98748347631</p>
                </div>
            </footer>
        </div>
    );
}

export default Footer;

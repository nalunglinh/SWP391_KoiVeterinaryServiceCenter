import React, { useEffect } from 'react';
import Header from '../components/Header';
import Footer from '../components/Footer';
import { Link } from 'react-router-dom';
import "../assets/css/LinkStandardCare.css"

function KoiFood() {
    useEffect(() => {
        window.scrollTo(0, 0)
      }, [])
    return (
        <>
            {/* Navigation Bar */}
            <Header />

            {/* Hero Section */}
            <section className="hero bg-light py-5">
                <div className="container text-center">
                    <h1 className="display-4">Standard Care for Koi Fish</h1>
                    <p className="lead mt-3">
                        Our Standard Care feature provides essential information on proper Koi
                        fish care. From daily maintenance tips to best practices for feeding,
                        cleaning, and monitoring health, this guide ensures your Koi receive the
                        highest standard of care to stay healthy and thrive.
                    </p>
                </div>
            </section>

            {/* Food for Koi Fish Section */}
            <section className="container my-5">
                <h1 className="text-center mb-4">Food for Koi Fish</h1>
                <div className="image-container d-flex justify-content-center align-items-center mb-4">
                    <img
                        src="public/img/koi12.webp"
                        alt="Food for Koi Fish"
                        className="img-fluid rounded"
                        style={{ maxWidth: '100%', height: 'auto', boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)' }}
                    />
                </div>
                {/* Navigation Links for Different Pages */}
               <div className="navigation-links">
                    <Link to="/StandardCare" className="btn btn-outline-primary btn-lg">StandardCare</Link>
                    <Link to="/AquaticPlant" className="btn btn-outline-primary btn-lg">AquaticPlant</Link>
                    <Link to="/KoiFood" className="btn btn-outline-primary btn-lg">KoiFood</Link>
                    <Link to="/KoiLifespan" className="btn btn-outline-primary btn-lg">KoiLifespan</Link>
                </div>
            </section>

            {/* Footer */}
            <Footer />
        </>
    );
}

export default KoiFood;

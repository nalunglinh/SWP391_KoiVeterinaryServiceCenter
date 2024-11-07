import React, { useEffect } from "react";
import { Link } from "react-router-dom";
import Header from "../components/Header";
import Footer from "../components/Footer";

function HomePage() {
  useEffect(() => {
    sessionStorage.clear();
  }, []);
  return (
    <>
      <Header />
      <div
        id="carouselExampleControls"
        className="carousel slide"
        data-bs-ride="carousel"
      >
        <div className="carousel-inner">
          <div className="carousel-item active" key="1">
            <div className="position-relative">
              <img
                src="../public/img/Koi-Login.jpg"
                className="d-block w-100"
                alt="Slide 1"
                style={{ filter: "blur(3px)" }} // Adjust blur amount as needed
              />
              <div className="col-md-6 gx-5 mb-4 position-absolute top-50 start-50 translate-middle text-center">
                <h4
                  className="text-white mb-4"
                  style={{ fontSize: "2rem", fontWeight: "bold" }}
                >
                  <strong>Welcome to Koi Health</strong>
                </h4>
                <h2 className="mb-4 text-white mb-4">
                  Where Dedicated Veterinary Experts Care for Your Koi Fish
                </h2>
                <p
                  className="text-white"
                  style={{ lineHeight: "1.6", fontSize: "1.1rem" }}
                >
                  At Koi Health, our professional veterinary team not only
                  possesses extensive knowledge of Koi fish but also brings
                  passion and dedication to every service. With years of
                  experience in Koi care and disease treatment, we are committed
                  to providing you with the best healthcare solutions to ensure
                  your Koi stay healthy and thrive.
                </p>
              </div>
            </div>
          </div>

          <div className="carousel-item" key="2">
            <img
              src="../public/img/koi1.jpg"
              className="d-block w-100"
              alt="Slide 2"
              style={{ filter: "blur(3px)" }}
            />
            <div className="col-md-6 gx-5 mb-4 position-absolute top-50 start-50 translate-middle text-center">
              <h4
                className="text-white mb-4"
                style={{ fontSize: "2rem", fontWeight: "bold" }}
              >
                <strong>
                  We understand that caring for Koi fish is not just about
                  treating diseases, but also about maintaining the best
                  possible living environment for them.
                </strong>
              </h4>
            </div>
          </div>
          <div className="carousel-item" key="3">
            <img
              src="../public/img/koi2.jpg"
              className="d-block w-100"
              alt="Slide 3"
              style={{ filter: "blur(3px)" }}
            />
            <div className="col-md-6 gx-5 mb-4 position-absolute top-50 start-50 translate-middle text-center">
              <h4
                className="text-white mb-4"
                style={{ fontSize: "2rem", fontWeight: "bold" }}
              >
                <strong>
                  We invite veterinarians, veterinary students, and fish health
                  experts to join our community in developing comprehensive care
                  solutions.
                </strong>
              </h4>
            </div>
          </div>
        </div>
        <button
          className="carousel-control-prev"
          type="button"
          data-bs-target="#carouselExampleControls"
          data-bs-slide="prev"
        >
          <span
            className="carousel-control-prev-icon"
            aria-hidden="true"
          ></span>
          <span className="visually-hidden">Previous</span>
        </button>
        <button
          className="carousel-control-next"
          type="button"
          data-bs-target="#carouselExampleControls"
          data-bs-slide="next"
        >
          <span
            className="carousel-control-next-icon"
            aria-hidden="true"
          ></span>
          <span className="visually-hidden">Next</span>
        </button>
      </div>

      <style>{`
        .link-hover:hover {
          text-decoration: underline;
          color: #007bff !important;
        }

        header {
          position: sticky;
          top: 0;
          z-index: 1000;
        }

        .carousel-item img {
          height: 500px;
          object-fit: cover;
        }

        .btn {
          transition: all 0.3s ease;
        }

        .btn:hover {
          transform: scale(1.05);
        }

        .shadow-sm {
          box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
        }

        .rounded-custom {
            border-radius: 15px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.2);
        }

        .mask-custom {
            background-color: rgba(0, 0, 0, 0.3);
        }

        .btn-custom {
            border-radius: 30px;
            padding: 10px 30px;
        }

        .invitation {
            background-color: #0c62c1;
            color: white;
            padding: 40px;
            margin: 30px auto;
            border-radius: 15px;
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
            text-align: center;
        }

        .btn-invitation {
            background-color: white;
            color: black;
            padding: 12px 30px;
            text-decoration: none;
            font-size: 1.1rem;
            border-radius: 5px;
            border: 2px solid #0c62c1;
            transition: all 0.3s ease;
        }

        .btn-invitation:hover {
            color: #0c62c1;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }
      `}</style>
      <main className="mt-5">
        <div className="container">
          {/* Section: Caring for Koi Fish */}
          <section className="py-5 bg-light">
            <div className="row align-items-center">
              <div className="col-md-6 ">
                <img
                  src="../public/img/koi4.jpg"
                  className="img-fluid rounded-custom"
                  alt="Koi Fish"
                />
              </div>
              <div className="col-md-6">
                <h4 className="text-primary mb-4" style={{ fontSize: "2rem" }}>
                  <strong>Caring for Koi Fish</strong>
                </h4>
                <p
                  className="text-muted"
                  style={{ lineHeight: "1.6", fontSize: "1.1rem" }}
                >
                  Our mission at Koi Health is to enhance the health management
                  of Koi fish, ensuring their well-being in both home ponds and
                  ornamental settings. We welcome veterinarians, veterinary
                  students, and fish health experts to join our community in
                  advancing Koi care.
                </p>

                <Link
                  to={`/StandardCare`}
                  className="btn btn-primary btn-lg mt-3"
                  style={{ borderRadius: 30, padding: "10px 30px" }}
                >
                  Read more
                </Link>
              </div>
            </div>
          </section>
          {/* Section: Veterinary Team */}
          <section className="py-5 bg-light">
            <div className="row align-items-center">
              <div className="col-md-6" style={{ paddingLeft: "38px" }}>
                <h4 className="text-primary mb-4" style={{ fontSize: "2rem" }}>
                  <strong>Meet Our Dedicated Veterinary Team</strong>
                </h4>
                <p
                  className="text-muted"
                  style={{ lineHeight: "1.6", fontSize: "1.1rem" }}
                >
                  Our dedicated veterinary team is at the heart of this mission.
                  With years of experience and a deep passion for Koi fish, our
                  veterinarians bring unmatched expertise and care to every
                  service.
                </p>
                <a href="#!" className="text-decoration-underline">
                  Click to learn more
                </a>
              </div>
              <div className="col-md-6">
                <img
                  src="../public/img/koi5.jpg"
                  className="img-fluid rounded-custom"
                  alt="Veterinary Team"
                />
              </div>
            </div>
          </section>
          {/* Section: Services for Koi Fish */}
          <section className="text-center py-5">
            <h4 className="mb-5">
              <strong>Our Services for Koi Fish</strong>
            </h4>
            <div className="row">
              <div className="col-lg-4 col-md-6 mb-4">
                <div className="card">
                  <img
                    src="https://plus.unsplash.com/premium_photo-1658506671316-0b293df7c72b"
                    className="card-img-top"
                    style={{
                      height: "300px",
                      objectFit: "cover",
                      borderRadius: "0.5rem 0.5rem 0 0", // Rounded corners on the top only
                    }}
                    alt="Consultation"
                  />
                  <div className="card-body">
                    <p className="card-text">
                      Consultation with a veterinarian online with doctor
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-lg-4 col-md-6 mb-4">
                <div className="card">
                  <img
                    src="https://images.unsplash.com/photo-1515986821211-0443679c2c9e"
                    className="card-img-top"
                    style={{
                      height: "300px",
                      objectFit: "cover",
                      borderRadius: "0.5rem 0.5rem 0 0", // Rounded corners on the top only
                    }}
                    alt="Pond Assessment"
                  />
                  <div className="card-body">
                    <p className="card-text">
                      Pond quality assessment and improvement
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-lg-4 col-md-6 mb-4">
                <div className="card">
                  <img
                    src="https://images.unsplash.com/photo-1511857543145-0c6865f84a50"
                    className="card-img-top"
                    style={{
                      height: "300px",
                      objectFit: "cover",
                      borderRadius: "0.5rem 0.5rem 0 0", // Rounded corners on the top only
                    }}
                    alt="Disease Treatment"
                  />
                  <div className="card-body">
                    <p className="card-text">
                      Appointment for Koi fish disease treatment
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </section>
          <section className="text-center py-5">
            <h4 className="mb-5">
              <strong>Blog For Month</strong>
            </h4>
            <div className="row">
              <div className="col-lg-4 col-md-6 mb-4">
                <div className="card">
                  <img
                    src="https://ishikoi.vn/storage/y8/gm/y8gmrrsgw90rtx9haa9vcu6a8bxf_dau_hieu_nhan_biet_ca_koi_ngua_minh.webp"
                    className="card-img-top"
                    style={{
                      height: "300px",
                      objectFit: "cover",
                      borderRadius: "0.5rem 0.5rem 0 0", // Rounded corners on the top only
                    }}
                    alt="Consultation"
                  />
                  <div className="card-body">
                    <p className="card-text">
                      <strong>
                        Common diseases in Koi fish - Anchor worms
                      </strong>
                    </p>
                    <p className="card-text">
                      Anchor Worm is a common disease in Koi fish caused by a
                      parasite called Lernaea. This parasite is shaped like an
                      anchor and attaches itself to the skin, gills, tail, fins,
                      eyes, mouth, etc. of Koi fish.
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-lg-4 col-md-6 mb-4">
                <div className="card">
                  <img
                    src="https://ishikoi.vn/storage/ha/36/ha36rxsqe551u5xilysvnjtvjc48_benh_thuong_gap_o_ca_koi_ran_nuoc.webp"
                    className="card-img-top"
                    style={{
                      height: "300px",
                      objectFit: "cover",
                      borderRadius: "0.5rem 0.5rem 0 0", // Rounded corners on the top only
                    }}
                    alt="Pond Assessment"
                  />
                  <div className="card-body">
                    <p className="card-text">
                      <strong>Water lice disease in Koi fish</strong>
                    </p>
                    <p className="card-text">
                      Water lice are one of the common diseases in Koi fish
                      caused by parasites. They parasitize Koi fish, suck blood
                      and nutrients, then transmit bacteria and viruses that
                      cause infection. The special thing about water lice is
                      that after parasitizing
                    </p>
                  </div>
                </div>
              </div>
              <div className="col-lg-4 col-md-6 mb-4">
                <div className="card">
                  <img
                    src="https://ishikoi.vn/storage/wq/v3/wqv34sdxfp965poojd4fj04x5ydd_benh_thuong_gap_o_ca_koi_san.webp"
                    className="card-img-top"
                    style={{
                      height: "300px",
                      objectFit: "cover",
                      borderRadius: "0.5rem 0.5rem 0 0", // Rounded corners on the top only
                    }}
                    alt="Disease Treatment"
                  />
                  <div className="card-body">
                    <p className="card-text">
                      <strong>
                        Common diseases in Koi fish - Skin flukes and gill
                        flukes
                      </strong>
                    </p>
                    <p className="card-text">
                      Skin flukes live on the entire skin of Koi fish, while
                      gill flukes only live on the gills. Skin flukes and gill
                      flukes live in two different locations, but the causes,
                      symptoms and treatments are relatively similar.
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </section>
          <div className="header container">
            <div className="vision-mission row">
              <div className="col-md-6 mb-4">
                <div className="vision-box p-4">
                  <h2 className="section-title">Our Vision</h2>
                  <p className="section-text">
                    Our vision is to offer expert health consultations, assess
                    and improve living environments, and treat illnesses for Koi
                    fish. The veterinary team at Koi Health is always ready to
                    work alongside you to ensure the best possible health for
                    your beloved Koi.
                  </p>
                </div>
              </div>
              <div className="col-md-6 mb-4">
                <div className="mission-box p-4">
                  <h2 className="section-title">Our Mission</h2>
                  <p className="section-text">
                    At Koi Health, our mission is to provide comprehensive and
                    professional care for Koi fish, ensuring they thrive and
                    live happily. We are committed to enhancing the quality of
                    life for Koi through top-tier medical services and dedicated
                    care.
                  </p>
                </div>
              </div>
            </div>
          </div>

          {/* Invitation Section */}
          <section className="invitation">
            <p>
              We warmly invite you to schedule a direct consultation with our
              experienced veterinary team at Koi Health. This is your chance to
              receive expert advice and tailored solutions for the health of
              your Koi fish. Book an appointment today to have our specialists
              assess your Koi’s health, provide guidance on their living
              environment, and offer effective care and treatment options.Let’s
              work together to ensure the long-term health and happiness of your
              cherished Koi!
            </p>
            <div className="button">
              <a href="/services" className="btn-invitation">
                Book a Service
              </a>
            </div>
          </section>
        </div>
      </main>
      <Footer />
    </>
  );
}

export default HomePage;

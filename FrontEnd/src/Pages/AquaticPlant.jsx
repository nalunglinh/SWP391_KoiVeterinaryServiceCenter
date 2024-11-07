import React, { useEffect } from "react";
import "../assets/css/styles1.css";
import Header from "../components/Header";
import Footer from "../components/Footer";
import { Link } from "react-router-dom";
import "../assets/css/LinkStandardCare.css";

function AquaticPlant() {
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);
  return (
    <>
      <Header></Header>
      {/* Hero Section */}
      <section
        className="hero"
        style={{
          backgroundImage:
            "url('https://images.unsplash.com/photo-1522567659205-ee20ba167aaa?q=80&w=1770&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D')",
          backgroundRepeat: "no-repeat",
          backgroundPosition: "center",
          backgroundSize: "cover",
        }}
      >
        <div className="hero-content">
          <h1>Standard Care for Koi Fish</h1>
          <p>
            Our Standard Care feature provides essential information on proper
            Koi fish care. From daily maintenance tips to best practices for
            feeding, cleaning, and monitoring health, this guide ensures your
            Koi receive the highest standard of care to stay healthy and thrive.
          </p>
        </div>
      </section>
      <section className="aquatic-plants">
        <h2>Aquatic Plant Ideas</h2>
        <p>
          Enhance your Koi pond with beautiful aquatic plants. They contribute
          to a natural ecosystem, helping your fish stay healthy and happy.
        </p>
        <div className="plant-card">
          <h3>Cryptocoryne Tropica</h3>
          <img
            src="public/img/koi1.jpg"
            alt="Picture of Cryptocoryne tropica"
          />
          <p>
            <em>Cryptocoryne tropica</em> is a popular aquatic plant from
            Southeast Asia, known for its unique wrinkled leaves. It grows to a
            height of 10-20 cm, with leaves that are usually bright green but
            can turn red or brown under different conditions. This plant thrives
            in 22-28°C water and a pH of 5.5-7.5, making it a versatile choice
            for enhancing the visual appeal of aquariums while providing a
            natural habitat for fish.
          </p>
        </div>
        <div className="plant-card">
          <h3>Water Hyacinth</h3>
          <img src="public/img/koi16.jpg" alt="Picture of Water Hyacinth" />
          <p>
            <em>Water Hyacinth</em> is a floating aquatic plant popular for Koi
            ponds and water gardens. It has round, glossy green leaves and
            beautiful purple-blue flowers that bloom during the warm seasons.
            This plant can grow up to 30 cm tall, and its roots provide
            excellent filtration, improving water quality by absorbing excess
            nutrients. Water Hyacinth thrives in warm temperatures (22-30°C) and
            can tolerate a wide range of water conditions, making it an
            effective and attractive addition to any pond. It also offers great
            shade and hiding spots for Koi fish
          </p>
        </div>
        <div className="plant-card">
          <h3>Lotus</h3>
          <img src="public/img/koi17.avif" alt="Picture of Lotus" />
          <p>
            <em>Lotus</em> is a striking aquatic plant known for its large,
            round leaves and beautiful pink or white flowers that rise above the
            water’s surface. It can grow up to 150 cm tall, making it ideal for
            larger Koi ponds. Lotus plants thrive in warm water (24-32°C) and
            full sunlight, providing shade for Koi and helping regulate water
            temperature. Their roots also aid in stabilizing the pond’s
            ecosystem, and the broad leaves offer natural cover for the fish.
          </p>
          <h3>Water Lily</h3>
          <img src="public/img/koi11.jpg" alt="Picture of Water Water Lily" />
          <p>
            <em>Water Lily</em> is a popular choice for both ornamental ponds
            and Koi habitats due to its vibrant, floating flowers that come in
            shades of white, pink, yellow, and blue. This plant remains close to
            the water&apos;s surface, with its flowers and leaves floating
            gracefully. Water Lilies grow in temperatures of 21-30°C and need
            plenty of sunlight to bloom fully. In addition to beautifying ponds,
            Water Lilies provide Koi with much-needed shade and shelter,
            creating a tranquil and balanced environment.
          </p>
        </div>
        {/* Navigation Links for Different Pages */}
        <div className="navigation-links">
          <Link to="/StandardCare" className="btn btn-outline-primary btn-lg">
            StandardCare
          </Link>
          <Link to="/AquaticPlant" className="btn btn-outline-primary btn-lg">
            AquaticPlant
          </Link>
          <Link to="/KoiFood" className="btn btn-outline-primary btn-lg">
            KoiFood
          </Link>
          <Link to="/KoiLifespan" className="btn btn-outline-primary btn-lg">
            KoiLifespan
          </Link>
        </div>
      </section>
      <Footer></Footer>
    </>
  );
}

export default AquaticPlant;

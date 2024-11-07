import React, { useEffect } from "react";
import "../assets/css/styles.css";
import Header from "../components/Header";
import Footer from "../components/Footer";
import { Link } from "react-router-dom";
import "../assets/css/LinkStandardCare.css";

function StandardCare() {
  useEffect(() => {
    window.scrollTo(0, 0);
  }, []);
  return (
    <div>
      <>
        {/* Navigation Section */}
        <meta charSet="UTF-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>Standard Care of Koi Fish</title>

        {/* Navigation */}
        <Header />
        {/* Hero Section */}
        <section className="hero">
          <div className="hero-content">
            <h1>Standard Care of Koi fish</h1>
            <p>
              Our Standard Care feature provides essential information on proper
              Koi fish care. From daily maintenance tips to best practices for
              feeding, cleaning, and monitoring health, this guide ensures your
              Koi receive the highest standard of care to stay healthy and
              thrive.
            </p>
          </div>
        </section>
        {/* Ideal of Pond Section */}
        <section className="pond-info">
          <h2>Ideal of Pond</h2>
          <p>
            When raising different types of koi fish, the water environment is a
            very important factor that determines the health and growth of the
            fish.
          </p>
          <table>
            <thead>
              <tr>
                <th>Parameter</th>
                <th>Range</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>Ammonia</td>
                <td>≤ 0.1 mg/L</td>
              </tr>
              <tr>
                <td>Nitrite</td>
                <td>0 mg/L</td>
              </tr>
              <tr>
                <td>Nitrate</td>
                <td> 20 mg/L</td>
              </tr>
              <tr>
                <td>pH</td>
                <td>6.5-8.5</td>
              </tr>
              <tr>
                <td>KH (alkalinity)</td>
                <td>&gt; 100 mg/L</td>
              </tr>
              <tr>
                <td>gH (total hardness)</td>
                <td>&gt; 100 mg/L</td>
              </tr>
              <tr>
                <td>Temperature</td>
                <td>Seasonal</td>
              </tr>
            </tbody>
          </table>
        </section>
        {/* Oxygen Section */}
        <section className="oxygen-info">
          <p>
            Dissolved oxygen: It is important to ensure that the oxygen level in
            the water is high enough, especially when the temperature increases.
            An aeration system or waterfall helps to increase oxygen.
          </p>
        </section>
        <meta charSet="UTF-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>Koi Fish Information</title>
        <link rel="stylesheet" href="styles.css" />
        <div className="container">
          {/* Gosanke */}
          <div className="fish-section">
            <h2 className="title">Gosanke (Kohaku, Sanke, Showa)</h2>
            <div className="fish-content">
              <div className="info">
                <p>
                  <strong>PH:</strong> 7.0 - 7.5
                </p>
                <p>
                  <strong>Temperature:</strong> 20°C - 25°C
                </p>
                <p>
                  <strong>Hardness (GH):</strong> 100 - 150 PPM
                </p>
                <p>
                  <strong>Alkalinity (KH):</strong> 70 - 140 PPM
                </p>
                <p>
                  <strong>Oxygen:</strong> Ensure good aeration to provide
                  adequate oxygen.
                </p>
              </div>
              <div className="image">
                <img src="public/img/KoiGosanke.png" alt="Gosanke Koi" />
              </div>
            </div>
          </div>
          {/* Utsurimono */}
          <div className="fish-section">
            <h2 className="title">
              Utsurimono (Shiro Utsuri, Hi Utsuri, Ki Utsuri)
            </h2>
            <div className="fish-content">
              <div className="info">
                <p>
                  <strong>PH:</strong> 7.0 - 7.5
                </p>
                <p>
                  <strong>Temperature:</strong> 18°C - 26°C
                </p>
                <p>
                  <strong>GH:</strong> 100 - 150 PPM
                </p>
                <p>
                  <strong>KH:</strong> 70 - 140 PPM
                </p>
                <p>
                  <strong>Ammonia, Nitrite:</strong> 0 PPM
                </p>
              </div>
              <div className="image">
                <img src="public/img/koi7.jpg" alt="Utsurimono Koi" />
              </div>
            </div>
          </div>
          {/* Bekko */}
          <div className="fish-section">
            <h2 className="title">Bekko (Shiro Bekko, Aka Bekko, Ki Bekko)</h2>
            <div className="fish-content">
              <div className="info">
                <p>
                  <strong>PH:</strong> 6.8 - 7.5
                </p>
                <p>
                  <strong>Temperature:</strong> 20°C - 25°C
                </p>
                <p>
                  <strong>GH:</strong> 100 - 150 PPM
                </p>
                <p>
                  <strong>KH:</strong> 70 - 140 PPM
                </p>
              </div>
              <div className="image">
                <img
                  src="https://koilover.vn/uploads/thumbs/8a5e8a05cda306fd5fb2_20211210151907320.jpg"
                  alt="Bekko Koi"
                />
              </div>
            </div>
          </div>
          {/* Asagi and Shusui */}
          <div className="fish-section">
            <h2 className="title">Asagi and Shusui</h2>
            <div className="fish-content">
              <div className="info">
                <p>
                  <strong>PH:</strong> 7.0 - 8.0
                </p>
                <p>
                  <strong>Temperature:</strong> 20°C - 25°C
                </p>
                <p>
                  <strong>GH:</strong> 100 - 150 PPM
                </p>
                <p>
                  <strong>KH:</strong> 70 - 150 PPM
                </p>
                <p>
                  <strong>Oxygen:</strong> Ensure high dissolved oxygen level as
                  Asagi and Shusui are sensitive to water conditions.
                </p>
              </div>
              <div className="image">
                <img
                  src="https://chocamekong.com/wp-content/uploads/2019/04/ca-koi-nhat-shusui.jpg"
                  alt="Asagi and Shusui Koi"
                />
              </div>
            </div>
          </div>
          {/*  Koromo */}
          <div className="fish-section">
            <h2 className="title">Koromo</h2>
            <div className="fish-content">
              <div className="info">
                <p>
                  <strong>PH:</strong> 7.0 - 7.5
                </p>
                <p>
                  <strong>Temperature:</strong> 20°C - 26°C
                </p>
                <p>
                  <strong>GH:</strong> 100 - 150 PPM
                </p>
                <p>
                  <strong>KH:</strong> 70 - 150 PPM
                </p>
              </div>
              <div className="image">
                <img src="public/img/koi8.jpg" alt="Koromo" />
              </div>
            </div>
          </div>
          {/*  Hikarimono */}
          <div className="fish-section">
            <h2 className="title">
              Hikarimono(Yamabuki Ogon, Platinum Ogon, Orenji Ogon)
            </h2>
            <div className="fish-content">
              <div className="info">
                <p>
                  <strong>PH:</strong> 7.0 - 7.5
                </p>
                <p>
                  <strong>Temperature:</strong> 20°C - 28°C (Cá koi ánh kim có
                  thể chịu được nhiệt độ cao hơn một chút)
                </p>
                <p>
                  <strong>GH:</strong> 100 - 150 PPM
                </p>
                <p>
                  <strong>KH:</strong> 70 - 150 PPM
                </p>
              </div>
              <div className="image">
                <img
                  src="public/img/koi9.png"
                  alt="Hikarimono(Yamabuki Ogon, Platinum Ogon, Orenji Ogon)"
                />
              </div>
            </div>
          </div>
          {/*  Doitsu Koi (Koi không vảy) */}
          <div className="fish-section">
            <h2 className="title">Doitsu Koi (Koi không vảy)</h2>
            <div className="fish-content">
              <div className="info">
                <p>
                  <strong>PH:</strong> 7.0 - 7.5
                </p>
                <p>
                  <strong>Temperature:</strong> 20°C - 25°C
                </p>
                <p>
                  <strong>GH:</strong> 100 - 150 PPM
                </p>
                <p>
                  <strong>KH:</strong> 70 - 150 PPM
                </p>
                <p>
                  <strong>Note:</strong> Due to their lack of scales, Doitsu koi
                  can be more sensitive to water conditions, especially clean
                  water quality.
                </p>
              </div>
              <div className="image">
                <img
                  src="public/img/koi10.webp"
                  alt="Doitsu Koi (Koi không vảy)"
                />
              </div>
            </div>
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
        </div>
        {/* Footer Section */}
        <Footer></Footer>
      </>
    </div>
  );
}

export default StandardCare;

import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import axios from "axios";
import Header from "../components/Header";
import Footer from "../components/Footer";
import "../assets/css/ProfileDoctor.css";

function ProfileDoctor() {
  const navigate = useNavigate();
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const id = queryParams.get("id");
  const CurrentUser = localStorage.getItem("CurrentUser");

  const [doctor, setDoctor] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [workShifts, setWorkShifts] = useState([]);
  const [service, setServices] = useState([]); // Thêm state để giữ dịch vụ
  const Service = JSON.parse(sessionStorage.getItem("service"));

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toISOString().split("T")[0];
  };

  useEffect(() => {
    const fetchDoctor = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5104/api/Doctor/Profile/${id}`,
          { withCredentials: true }
        );
        setDoctor(response.data);

  
        
        const responseWorkShifts = await axios.get(
          `http://localhost:5104/api/Doctor/Doctor-workshift/${id}`
        );
        setWorkShifts(responseWorkShifts.data);
      } catch (err) {
        console.error("Error fetching data:", err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchDoctor();
  }, [id]);

  const handleBooking = async () => {
    if (!CurrentUser) {

      sessionStorage.setItem("redirectPath", "/profile-doctor");
      
      navigate("/login");
      return;
    }

    // Lưu responseServices vào sessionStorage
    const getservice = await axios.get(
      `http://localhost:5104/api/Doctor/get-services-by-doctor/${id}`
    );
    const service = getservice.data.$values[0];
    sessionStorage.setItem("service", JSON.stringify(service));

    navigate(`/Consultpayment?doctorId=${id}`);
  };

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <>
      <Header />
      <main>
        <section className="profile">
          <h1>Doctor&apos;s Profile</h1>
          <div className="profile-container">
            <div className="profile-item">
              <div className="profile-img">
                <img
                  src={`/img/Doctor${doctor.doctorId}.png`}
                  alt={doctor.fullName}
                  onError={(e) => {
                    e.target.onerror = null;
                    e.target.src = "/img/default-image.jpg";
                  }}
                />
              </div>
              <div className="profile-info">
                <h2>{doctor.fullName}</h2>
                <p>Email: {doctor.email}</p>
                <p>Address: {doctor.userAddress}</p>
                <p>Phone: {doctor.phone}</p>
                <p>DOB: {formatDate(doctor.dob)}</p>
                <p>Workplace: Veterinary Service center</p>
              </div>
              <div className="doctor-actions">
                <button
                  type="button"
                  onClick={handleBooking}
                  className="book-doctor-btn"
                >
                  Book doctor now
                </button>
                <button
                  className="see-another-doctor-link"
                  onClick={() => navigate(-1)}
                >
                  See another doctor
                </button>
              </div>
            </div>
          </div>
        </section>
      </main>
      <Footer />
    </>
  );
}

export default ProfileDoctor;

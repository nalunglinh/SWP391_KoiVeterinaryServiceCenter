import { useEffect, useState } from "react";
import "../assets/css/Consultation.css";
import { Link, useLocation } from "react-router-dom";
import Header from "../components/Header";
import Footer from "../components/Footer";
import axios from "axios";

function Consultation() {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const serviceId = queryParams.get("serviceId");

  const [data, setData] = useState([]); // Danh sách bác sĩ
  const [error, setError] = useState(null); // Lỗi khi fetch dữ liệu
  const [loading, setLoading] = useState(true); // Trạng thái tải dữ liệu
  const [searchTerm, setSearchTerm] = useState(""); // Từ tìm kiếm

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        console.log("serviceId", serviceId);
        const response = await axios.get(
          `http://localhost:5104/api/Doctor/ListByService/${serviceId}`
        );
        const Doctors = response.data.$values
        setData(Doctors); // Cập nhật danh sách bác sĩ
      } catch (err) {
        console.error("Error fetching data:", err);
        setError(err);
      } finally {
        setLoading(false); // Kết thúc quá trình tải
      }
    };

    fetchData(); // Gọi hàm fetchData
  }, [serviceId]); // Chạy lại khi serviceId thay đổi

  const getImageUrl = (doctorId) => {
    return `/img/Doctor${doctorId}.png`; // URL hình ảnh bác sĩ
  };

  // Lọc danh sách bác sĩ theo từ tìm kiếm
  const filteredDoctors = data.filter((doctor) =>
    doctor.fullName.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <>
      <Header />
      <div className="container" style={{ display: "flex", margin: "20px" }}>
        <aside>
          <ul>
            <li>Other service</li>
            <li>
              <Link to="/Pond?serviceId=2">Pond quality assessment</Link>
            </li>
            <li>
              <Link to="/Pond?serviceId=3">
                Appointment for fish disease treatment
              </Link>
            </li>
            <li>
              <Link to="/">Home Page</Link>
            </li>
            <li>
              <Link to="/Services">Services</Link>
            </li>
          </ul>
        </aside>
        <main style={{ width: "75%", marginLeft: "5%" }}>
          <h1>Doctors</h1>

          <div className="filter-section">
            {/* Search Box */}
            <div className="search-box">
              <input
                type="search"
                placeholder="Search doctor by name"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)} // Cập nhật từ tìm kiếm
              />
            </div>
          </div>

          <div className="doctor-list">
            {error && (
              <p className="error-message">fetching error: {error.message}</p>
            )}
            {loading ? (
              <p>loading...</p> // Thông báo đang tải
            ) : filteredDoctors.length > 0 ? (
              filteredDoctors.map((doctor) => (
                <Link
                  to={`/profile-doctor?id=${doctor.doctorId}`}
                  key={doctor.doctorId}
                >
                  <div className="doctor-card">
                    <img
                      src={getImageUrl(doctor.doctorId)}
                      alt={`Doctor ${doctor.fullName}`}
                      onError={(e) => {
                        e.target.onerror = null;
                        e.target.src = "/img/default-image.jpg"; // Hình ảnh mặc định nếu không có
                      }}
                    />
                    <h3>{doctor.fullName}</h3>
                    <p>{doctor.userAddress}</p>
                  </div>
                </Link>
              ))
            ) : (
              <p>No Doctor available</p> 
            )}
          </div>
        </main>
      </div>
      <Footer />
    </>
  );
}

export default Consultation;

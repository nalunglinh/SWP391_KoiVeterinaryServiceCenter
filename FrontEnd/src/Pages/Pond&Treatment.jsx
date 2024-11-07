import React, { useEffect, useState } from "react";
import Header from "../components/Header";
import "../assets/css/Pond.css";
import { Link, useLocation, useNavigate } from "react-router-dom";
import axios from "axios";
import "../assets/css/workshift.css";

function Pond() {
  const CurrentUser = JSON.parse(localStorage.getItem("CurrentUser"));
  const location = useLocation();
  const navigate = useNavigate();
  const queryParams = new URLSearchParams(location.search);
  const serviceId = queryParams.get("serviceId");
  const [listdoctors, setlistdoctors] = useState([]);
  const [selectedDay, setSelectedDay] = useState(null);
  const nagative = useNavigate();
  const [searchTerm, setSearchTerm] = useState("");

  const fetch = async () => {
    try {
      
      const response = await axios.get(
        `http://localhost:5104/api/Doctor/Schedules-by-Service/${serviceId}`
      );

      // check booked thì ko trả về lịch đó nữa 
      const workshifts = response.data.$values;
      const doctorArray = await axios.get(
        `http://localhost:5104/api/Doctor/ListByService/${serviceId}`
      );
      const doctors = doctorArray.data.$values;
      console.log(workshifts);
      const mergeDoctorsWithWorkshifts = (doctors, workshifts) => {
        return doctors.map((doctor) => {
          const doctorWorkshifts = workshifts.find(
            (ws) => ws.doctorId === doctor.doctorId
          );

          return {
            ...doctor,
            workshifts: doctorWorkshifts
              ? doctorWorkshifts.workshifts.$values
              : [],
          };
        });
      };

      const mergedData = mergeDoctorsWithWorkshifts(doctors, workshifts);
      setlistdoctors(mergedData);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    if (serviceId) {
      fetch();
    } else {
      console.log("Service ID is not defined");
    }
  }, [serviceId]);

  const daysOfWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

  // Hàm xử lý khi click vào workshift
  const handleClick = async(workshift, doctorId) => {

    console.log("Selected Workshift:", workshift);
    sessionStorage.setItem("workshift", JSON.stringify(workshift));
    const getservice = await axios.get(
      `http://localhost:5104/api/Doctor/get-services-by-doctor/${doctorId}`
    
    );
    const service = (getservice.data.$values[0])
    sessionStorage.setItem("service", JSON.stringify(service))
    nagative(`/profile-doctor?id=${doctorId}`)

  };

  return (
    <>
    
      <Header />
      <div
        className="container"
        style={{
          display: "flex",
          justifyContent: "space-between",
          padding: "20px",
        }}
      >
      
        <div className="doctor-section">
          <h2>
            <Link to="/Services"> Services</Link> &gt;{" "}
            <span style={{ color: "coral" }}>Pond water assessment doctor</span>
          </h2>
          <div className="filter-section">
            {/* Search Box */}
            <div className="search-box">
              <input
                type="search"
                placeholder="Search doctor by name"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)} 
              />
            </div>
          </div>

       

          <ul className="tagList">
            {daysOfWeek.map((day) => (
              <li
                key={day}
                className={day.toLowerCase()}
                onClick={() => setSelectedDay(day)}
                style={{
                  cursor: "pointer",
                  backgroundColor: selectedDay === day ? "#ccc" : "initial",
                }}
              >
                <span>{day}</span>
              </li>
            ))}
          </ul>

              
          <div className="doctor-list">
            {Array.isArray(listdoctors) && listdoctors.length > 0 ? (
              listdoctors.filter((doctor => 
                doctor.fullName.toLowerCase().includes(searchTerm.toLowerCase()))) 
                .map((doctor) => (
                <div key={doctor.doctorId} className="doctor-card">
                  <div className="doctor-info">
                    <h3>{doctor.fullName}</h3>

                    <div className="appointment-times">
                      {doctor.workshifts
                        .filter((time) => time.dayOfWeek === selectedDay)
                        .map((time, index) => (
                          <button
                            onClick={() => handleClick(time, doctor.doctorId)}
                            className="workshift"
                            key={index}
                          >{`${time.timeFrom} - ${time.timeTo}`}</button>
                        ))}
                    </div>
                  </div>
                  {/* <Link to="/WaterPond" className="book-now">
                    Book now
                  </Link> */}
                </div>
              ))
            ) : (
              <p>No doctors available</p>
            )}
          </div>
        </div>
      </div>
    </>
  );
}

export default Pond;

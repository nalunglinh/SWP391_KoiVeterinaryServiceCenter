import React, { useEffect, useState } from "react";
import Header from "../components/Header";
import Footer from "../components/Footer";
import { Form, Link, useLocation, useNavigate } from "react-router-dom";
import axios from "axios";
import "../assets/css/payment.css";

function Consult() {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const doctorId = queryParams.get("doctorId");
  const [service, setService] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedDate, setSelectedDate] = useState("");
  const [isHomeVisit, setIsHomeVisit] = useState(false);
  const [place, setPlace] = useState("");
  const CurrentUser = JSON.parse(localStorage.getItem("CurrentUser"));
  const navigate = useNavigate();
  const Workshift = JSON.parse(sessionStorage.getItem("workshift"));
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Lấy dịch vụ từ sessionStorage
  const serviceData = JSON.parse(sessionStorage.getItem("service"));
  const servicePrice = serviceData?.price;
  const serviceSurcharge = serviceData?.surcharge;

  const totalPrice = () => {
    let total = servicePrice;

    if (isHomeVisit) {
      total += serviceSurcharge;
    }
    return total;
  };

  const Update = async (objectUpdate) => {
    try {
      await axios.put(
        "http://localhost:5104/api/Account/edit-profile",
        objectUpdate,
        { withCredentials: true }
      );
    } catch (error) {
      console.error("There was an error updating the profile!", error);
    }
  };

  const CreateBookingService1 = async (BookingInfo) => {
    try {
      await axios.post(
        `http://localhost:5104/api/Doctor/Booking/service-1/${doctorId}`,
        BookingInfo,
        { withCredentials: true }
      );
    } catch (error) {
      console.error("Error creating booking:", error);
    }
  };

  const CreateBookingServiceOther = async (BookingInfo) => {
    try {
      await axios.post(
        `http://localhost:5104/api/Doctor/Booking/Service-2-3/${doctorId}?workshiftId=${Workshift?.workshiftId}`,
        BookingInfo,
        { withCredentials: true }
      );
    } catch (error) {
      console.error("Error creating booking:", error);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (isSubmitting) return;
    setIsSubmitting(true);

    const form = new FormData(e.target);
    const fullName = form.get("fullName");
    const phoneNumber = form.get("phoneNumber");
    const description = form.get("description");
    const appointmentDate = form.get("appointmentDate");

    const objectUpdate = {
      userId: CurrentUser?.userId,
      fullName,
      phone: phoneNumber,
      email: CurrentUser?.email,
      dob: CurrentUser?.dob,
      userAddress: CurrentUser?.userAddress,
    };

    let BookingInfo;
    if (service && service.serviceId === 1) {
      BookingInfo = {
        description,
        appointmentDate,
        customerId: CurrentUser?.userId,
        serviceId: service.serviceId,
      };
      await CreateBookingService1(BookingInfo);
    } else {
      BookingInfo = {
        description,
        appointmentDate,
        customerId: CurrentUser?.userId,
        place: isHomeVisit ? place : undefined, // Chỉ gửi place khi chọn Home visit
        isHomeVisit,
        serviceId: service.serviceId,
      };
      await CreateBookingServiceOther(BookingInfo);
    }

    sessionStorage.setItem("service", JSON.stringify(service));
    await Update(objectUpdate);
    navigate("/Tpayment");
  };

  const handleDateChange = (event) => {
    setSelectedDate(event.target.value);
  };

  useEffect(() => {
    const fetchServiceByDoctorId = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5104/api/Doctor/get-services-by-doctor/${doctorId}`
        );
        setService(response.data.$values[0]);
      } catch (err) {
        console.error("Error fetching data:", err);
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchServiceByDoctorId();
  }, [doctorId]);

  return (
    <>
      <div className="payment-page">
        <Header />
        <div className="container mt-5">
          <nav aria-label="breadcrumb">
            <ol className="breadcrumb">
              <li className="breadcrumb-item">
                <Link to="/Services">Services</Link>
              </li>
              <li className="breadcrumb-item active" aria-current="page">
                Payment
              </li>
            </ol>
          </nav>
          <h1 className="display-5">Direct Veterinary Consultation Service</h1>
          <p>
            Welcome to our direct consulting service! We are always ready to
            support you in all situations related to caring for and treating Koi
            fish.
          </p>
          <p>
            To make the consultation session most effective, please prepare the
            following information:
          </p>
          <ul>
            <li>Basic information about your Koi fish</li>
            <li>
              Information about the pond or aquarium (capacity, number of fish,
              etc.)
            </li>
            <li>
              Water quality parameters (ammonia, nitrite, nitrate, pH, water
              hardness, temperature, salinity)
            </li>
            <li>
              Illustrative images or videos (sending videos is encouraged)
            </li>
          </ul>
          <p>
            We will review the diagnoses together, provide a list of
            possibilities, and suggest appropriate treatment methods. If
            necessary, we will connect you with veterinarians who specialize in
            aquatics in your area for further in-depth support.
          </p>
          <form
            onSubmit={handleSubmit}
            className="text-white p-4 rounded mb-5"
            style={{ backgroundColor: "#19579a" }}
          >
            <div className="row mb-3">
              <div className="col-md-6">
                <label htmlFor="fullName" className="form-label">
                  Full name
                </label>
                <input
                  defaultValue={CurrentUser.fullName}
                  type="text"
                  className="form-control"
                  id="fullName"
                  name="fullName"
                  placeholder="Up to 32 characters"
                  required
                />
              </div>
              <div className="col-md-6">
                <label htmlFor="appointmentDate" className="form-label">
                  Appointment Date
                </label>
                <input
                  className="form-control"
                  type="date"
                  id="appointmentDate"
                  name="appointmentDate"
                  value={selectedDate}
                  onChange={handleDateChange}
                  required
                />
              </div>
            </div>
            <div className="row mb-3">
              <div className="col-md-6">
                <label htmlFor="phoneNumber" className="form-label">
                  Phone number
                </label>
                <input
                  defaultValue={CurrentUser.phone}
                  type="tel"
                  className="form-control"
                  id="phoneNumber"
                  name="phoneNumber"
                  placeholder="At least 10 - 11 numbers"
                  required
                />
              </div>
            </div>
            <div className="row mb-3">
              <label htmlFor="description" className="form-label">
                Description
              </label>
              <textarea
                className="form-control"
                id="description"
                name="description"
                rows="3"
                placeholder="Enter your characters"
                required
              />
            </div>
            <div className="row mb-3">
              <div className="col-md-6">
                <label htmlFor="place" className="form-label">
                  Place of consultation
                </label>
                <input
                  type="text"
                  className="form-control"
                  id="place"
                  name="place"
                  onChange={(e) => setPlace(e.target.value)}
                  disabled={!isHomeVisit}
                />
                <div className="form-check mt-3">
                  <input
                    className="form-check-input"
                    type="checkbox"
                    id="isHomeVisit"
                    name="isHomeVisit"
                    value="true"
                    onChange={(e) => setIsHomeVisit(e.target.checked)}
                  />
                  <label className="form-check-label" htmlFor="isHomeVisit">
                    Home Visit Service
                  </label>
                </div>
              </div>
            </div>
            <div className="row mb-3">
            
                <div className="payment-summary">
                  <h7>service: {service?.serviceName}</h7>
                  <h5>Total: {totalPrice().toFixed(2)} $</h5>
                </div>
              </div>
              <div className="payment-button">
                <button type="submit" className="btn btn-primary">
                  {isSubmitting ? "Submitting..." : "Confirm"}
                </button>
              </div>
         
          </form>
        </div>
      </div>
      <Footer />
    </>
  );
}

export default Consult;

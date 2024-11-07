import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function Payment() {
  const [qrCode, setQrCode] = useState(null);
  const [showModal, setShowModal] = useState(false);
  const [billData, setBillData] = useState(null); // State to store bill information
  const CurrentUser = JSON.parse(localStorage.getItem("CurrentUser"));
  const service = JSON.parse(sessionStorage.getItem("service"));
  const navigate = useNavigate();

  useEffect(() => {
    let isPaymentCreated = false;

    const fetchQrCode = async () => {
      if (isPaymentCreated) return;
      isPaymentCreated = true;

      try {
        const getAllAppointments = await axios.get(
          "http://localhost:5104/api/Appointment/All",
          { withCredentials: true }
        );

        const allPayments = getAllAppointments.data?.$values || [];
        const maxAppointmentId = Math.max(
          ...allPayments.map((a) => a.appointmentId)
        );
        const latestAppointment = allPayments.find(
          (a) => a.appointmentId === maxAppointmentId
        );

        if (latestAppointment) {
          const createPaymentData = {
            appointmentId: latestAppointment.appointmentId,
            fullName: CurrentUser.fullName,
            phone: CurrentUser.phone,
            email: CurrentUser.email,
            userAddress: CurrentUser.userAddress,
            paymentMethod: "VIETQR",
            isHomeVisit: service.serviceId === 1 ? false : true,
          };

          const response = await axios.post(
            `http://localhost:5104/api/Payment/create-payment?appointmentId=${createPaymentData.appointmentId}`,
            createPaymentData,
            { withCredentials: true }
          );

          const paymentId = response.data.paymentId;

          const qrRequestData = {
            paymentId: paymentId,
            customerId: CurrentUser.userId,
          };
          const generateQR = await axios.post(
            `http://localhost:5104/api/Payment/vietqr/mock/pay`,
            qrRequestData,
            { withCredentials: true }
          );
          const QR = generateQR.data.qrCodeUrl;
          setQrCode(QR);

          setTimeout(async () => {
            setShowModal(true);
            try {
              const billResponse = await axios.post(
                `http://localhost:5104/api/Payment/Bill/${paymentId}`,
                {},
                { withCredentials: true }
              );
              setBillData(billResponse.data); // Store bill data in state
            } catch (error) {
              console.error("Error fetching bill:", error);
            }
          }, 5000);
        } else {
          console.log("No appointments found.");
        }
      } catch (error) {
        console.error("Error fetching appointments or creating payment:", error);
      }
    };

    fetchQrCode();
  }, []);

  const handleOk = () => {
    sessionStorage.removeItem("service");
    sessionStorage.removeItem("workshift");
    setShowModal(false);
    navigate("/");
  };

  return (
    <>
      <section className="payment-method bg-light p-4 rounded shadow-sm mb-5">
        <div className="row">
          <div className="col-md-6 d-flex justify-content-center align-items-center">
            {qrCode && <img src={qrCode} alt="QR Code" className="qr-code" />}
          </div>
        </div>
      </section>

      {showModal && billData && (
        <div className="modal">
          <div className="modal-content">
            <span className="close" onClick={handleOk}>
              &times;
            </span>
            <h2>Payment Successful</h2>
            <p>Your payment has been successfully processed!</p>
            <h3>Bill Details:</h3>
            <p>Bill ID: {billData.billId}</p>
            <p>Customer Name: {billData.fullName}</p>
            <p>Service: {billData.serviceName}</p>
            <p>Bill Date: {new Date(billData.billDate).toLocaleString()}</p>
            <p>Total Amount: {billData.totalAmount} $</p>
            <button onClick={handleOk}>OK</button>
          </div>
        </div>
      )}

      <style>
        {`
          .modal {
            display: block;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0, 0, 0, 0.7);
          }

          .modal-content {
            background-color: #fff;
            margin: 15% auto;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.3);
            width: 80%;
            max-width: 500px;
            animation: fadeIn 0.3s;
          }

          .close {
            color: #aaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
          }

          .close:hover,
          .close:focus {
            color: black;
            text-decoration: none;
            cursor: pointer;
          }

          @keyframes fadeIn {
            from {
              opacity: 0;
            }
            to {
              opacity: 1;
            }
          }
        `}
      </style>
    </>
  );
}

export default Payment;

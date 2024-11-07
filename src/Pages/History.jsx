import React, { useEffect, useState } from "react";
import axios from "axios";
import Header from "../components/Header";
import Footer from "../components/Footer";
import { Modal, Button, Input, Rate, message } from "antd";
import "../assets/css/History.css";

const History = () => {
  const [historyData, setHistoryData] = useState([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [feedback, setFeedback] = useState({
    ratingValue: 5,
    comment: "",
    appointmentId: null,
    doctorId: null,
    serviceId: null,
  });
  const CurrentUser = JSON.parse(localStorage.getItem("CurrentUser"));

  useEffect(() => {
    const fetchHistory = async () => {
      try {
        const response = await axios.get(
          `http://localhost:5104/api/History/Customer/${CurrentUser.userId}`
        );
        setHistoryData(response.data.$values || []);
      } catch (error) {
        console.error("Error fetching history data:", error);
        message.error("Error fetching history data.");
      }
    };
    fetchHistory();
  }, []);

  const showModal = (appointmentId, doctorId, serviceId) => {
    setFeedback({ ...feedback, appointmentId, doctorId, serviceId });
    setIsModalVisible(true);
  };

  const handleFeedbackSubmit = async () => {
    const feedbackData = {
      appointmentId: feedback.appointmentId,
      customerId: CurrentUser.userId,
      doctorId: feedback.doctorId,
      serviceId: feedback.serviceId,
      comment: feedback.comment,
      rating: feedback.ratingValue,
    };
  
    console.log("Sending feedback data:", feedbackData);  // Kiểm tra dữ liệu gửi lên
  
    try {
      await axios.post("http://localhost:5104/api/ServiceFeedback/CreateFeedback", feedbackData);
      message.success("Feedback submitted successfully!");
      setIsModalVisible(false);
    } catch (error) {
      console.error("Error submitting feedback:", error);
      message.error("An error occurred while submitting feedback.");
    }
  };


  return (
    <>
      <Header />
      <div>
        <h1>Payment History</h1>
        <table>
          <thead>
            <tr>
              <th>Payment Method</th>
              <th>Amount</th>
              <th>Full Name</th>
              <th>Email</th>
              <th>Service Name</th>
              <th>Doctor Name</th>
              <th>Description</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {historyData.map((item) => (
              <tr key={item.paymentId}>
                <td>{item.paymentMethod}</td>
                <td>{item.amount}</td>
                <td>{item.customerName}</td>
                <td>{item.customerEmail}</td>
                <td>{item.serviceName}</td>
                <td>{item.doctorName}</td>
                <td>{item.description}</td>
                <td>
                  <Button onClick={() => showModal(item.appointmentId, item.doctorId, item.serviceId)}>
                    Send Feedback
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <Modal
        title="Send Feedback"
        visible={isModalVisible}
        onOk={handleFeedbackSubmit}
        onCancel={() => setIsModalVisible(false)}
      >
        <div>
          <label>Feedback:</label>
          <Rate
            value={feedback.ratingValue}
            onChange={(value) => setFeedback({ ...feedback, ratingValue: value })}
          />
        </div>
        <div style={{ marginTop: "10px" }}>
          <label>Comment:</label>
          <Input.TextArea
            rows={4}
            value={feedback.comment}
            onChange={(e) => setFeedback({ ...feedback, comment: e.target.value })}
          />
        </div>
      </Modal>
      <Footer />
    </>
  );
};

export default History;

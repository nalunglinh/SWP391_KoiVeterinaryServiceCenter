import { useEffect, useState } from 'react';
import Header from '../components/Header';
import Footer from '../components/Footer';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function Services() {
    useEffect(() => {
        sessionStorage.clear();
      }, []);
    const [data, setData] = useState([]); // State để giữ dữ liệu phản hồi từ API
    const [error, setError] = useState(null); // State để giữ lỗi nếu có
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await axios.get('http://localhost:5104/api/Service/index');
                const service = response.data.$values;
                setData(service); // Đặt dữ liệu nhận được vào state
            } catch (err) {
                console.error("Error fetching data:", err); // Log lỗi
                setError(err); // Đặt lỗi vào state
            }
        };

        fetchData(); // Gọi hàm để lấy dữ liệu
    }, []);

    // Hàm xử lý khi nhấn vào nút
    const handleServiceClick = (service) => {
        if (service.serviceId === 1) {
          
            navigate(`/Consultation?serviceId=${service.serviceId}`); 
        } else if (service.serviceId === 2) {
            navigate(`/pond?serviceId=${service.serviceId}`); 
        } else {
            navigate(`/pond?serviceId=${service.serviceId}`); 
        }
    };

    return (
        <>
            <Header />
            <div className="container my-5">
                <h1 className="text-center mb-5 display-4">Our Services</h1>
                <div className="row">
                    {error ? (
                        <p className="text-danger">Failed to load services. Please try again later.</p>
                    ) : data.length > 0 ? (
                        data.map((service) => (
                            <div key={service.serviceId} className="col-md-4 mb-4">
                                <div className="card h-100 shadow-sm border-0 rounded-lg overflow-hidden service-card">
                                    <div className="card-body text-center p-4">
                                        <div className="service-icon mb-3">
                                            <i className={`text-primary fs-1`}></i> {/* Thay thế icon nếu cần */}
                                        </div>
                                        <h5 className="card-title fw-bold mb-3">{service.serviceName}</h5>
                                        <p className="card-text text-muted">{service.description}</p>
                                    </div>
                                    <div className="card-footer text-center bg-primary text-white py-3">
                                        <button 
                                            onClick={() => handleServiceClick(service)} 
                                            className="btn btn-light btn-sm fw-bold m-1"
                                        >
                                            {service.serviceId === 1 ? 'Booking Doctor' : 'Booking Doctor'}
                                        </button>
                                    </div>
                                </div>
                            </div>
                        ))
                    ) : (
                        <p>No services available</p>
                    )}
                </div>
            </div>
            <Footer />
        </>
    );
}

export default Services;

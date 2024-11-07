import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Header from '../components/Header';
import Footer from '../components/Footer';
import { Link } from 'react-router-dom';

function Profile() {
    const [profile, setProfile] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        // Fetch profile data from the API with credentials
        axios.get('http://localhost:5104/api/Account/profile', { withCredentials: true })
            .then(response => {
                setProfile(response.data);
            })
            .catch(error => {
                console.error("There was an error fetching the profile data!", error);
                setError("Unable to fetch profile data.");
            })
            .finally(() => {
                setLoading(false);
            });
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <>
            <Header />
            <div className="container mt-5">
                <div className="row justify-content-center">
                    <div className="col-md-6">
                        {/* Profile Card */}
                        <div className="card shadow-lg border-0 rounded-lg">
                            <div className="card-header bg-primary text-white text-center">
                                <h3 className="mb-0">User Profile</h3>
                            </div>
                            <div className="card-body">
                                {/* User Information */}
                                <ul className="list-group list-group-flush text-left">
                                    <li className="list-group-item d-flex justify-content-between align-items-center">
                                        <strong>Full Name:</strong> <span>{profile?.fullName || 'N/A'}</span>
                                    </li>
                                    <li className="list-group-item d-flex justify-content-between align-items-center">
                                        <strong>Email:</strong> <span>{profile?.email || 'N/A'}</span>
                                    </li>
                                    <li className="list-group-item d-flex justify-content-between align-items-center">
                                        <strong>Phone:</strong> <span>{profile?.phone || 'N/A'}</span>
                                    </li>
                                    <li className="list-group-item d-flex justify-content-between align-items-center">
                                        <strong>Date of Birth:</strong> <span>{profile?.dob ? new Date(profile.dob).toLocaleDateString() : 'N/A'}</span>
                                    </li>
                                    <li className="list-group-item d-flex justify-content-between align-items-center">
                                        <strong>User Address:</strong> <span>{profile?.userAddress || 'N/A'}</span>
                                    </li>
                                </ul>
                                {/* Link to Edit Profile */}
                                <div className="text-center mt-3">
                                    <Link to="/edit-profile" className="btn btn-warning">Edit Profile</Link>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <Footer />
        </>
    );
    
}

export default Profile;

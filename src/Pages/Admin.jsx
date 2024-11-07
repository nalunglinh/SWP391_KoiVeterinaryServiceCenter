import React from 'react'
import Header from '../components/Header'
import Footer from '../components/Footer'

function Admin() {
    return (
        <> <Header></Header>
        <div className="d-flex">
            {/* Sidebar */}
            <nav className="sidebar p-3">
                <h4 className="text-white">Admin Page</h4>
                <ul className="nav flex-column">
                    <li className="nav-item">
                        <a className="nav-link" href="#">
                            Edit Customer
                        </a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="#">
                            Edit doctor
                        </a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="#">
                            Logout
                        </a>
                    </li>
                </ul>
            </nav>
            <div className="main-content p-4" style={{ flex: 1 }}>
                <header className="d-flex justify-content-between align-items-center mb-4">
                    <h2>Dashboard</h2>
                    <button className="btn btn-outline-secondary">User Profile</button>
                </header>
                <div className="row">
                    <div className="col-md-4 mb-4">
                        <div className="card">
                            <div className="card-body">
                                <h5 className="card-title">Total Users</h5>
                                <p className="card-text">150</p>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-4 mb-4">
                        <div className="card">
                            <div className="card-body">
                                <h5 className="card-title">Active Users</h5>
                                <p className="card-text">120</p>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-4 mb-4">
                        <div className="card">
                            <div className="card-body">
                                <h5 className="card-title">Pending Requests</h5>
                                <p className="card-text">5</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <Footer></Footer>
        </div>
        </>
    )
}

export default Admin
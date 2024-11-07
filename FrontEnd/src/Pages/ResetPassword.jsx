import React from 'react'
import { Link } from 'react-router-dom';

function ResetPassword() {
    return (
        <><div className="container">
            <div className="row justify-content-center">
                <div className="col-md-6">
                    <div className="card shadow p-4">
                        <h3 className="text-center mb-4">Reset Password</h3>
                        <p className="text-muted text-center">Enter your new password below</p>
                        <form action="#" method="POST">
                            <div className="mb-3">
                                <label htmlFor="new-password" className="form-label">
                                    New Password
                                </label>
                                <input
                                    type="password"
                                    className="form-control"
                                    id="new-password"
                                    placeholder="New password"
                                    required=""
                                />
                            </div>
                            <div className="mb-3">
                                <label htmlFor="confirm-password" className="form-label">
                                    Confirm Password
                                </label>
                                <input
                                    type="password"
                                    className="form-control"
                                    id="confirm-password"
                                    placeholder="Confirm password"
                                    required=""
                                />
                            </div>
                            <button type="submit" className="btn btn-primary w-100">
                                <Link to={`/login`} className="btn btn-primary w-100">Reset Password</Link>

                            </button>
                        </form>
                        <div className="text-center mt-3">
                            <Link to={`/login`} className="btn btn-outline-primary me-2 rounded-pill px-4">Back to Login</Link>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </>
    )
}

export default ResetPassword
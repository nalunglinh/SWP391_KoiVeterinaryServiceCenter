import { useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'; // Ensure you import BrowserRouter
import HomePage from './Pages/HomePage.jsx';
import SignIn from './Pages/SignIn.jsx';
import SignUp from './Pages/SignUp.jsx';
import StandardCare from './Pages/StandardCare.jsx';
import AquaticPlant from './Pages/AquaticPlant.jsx';
import KoiFood from './Pages/KoiFood.jsx';
import KoiLifespan from './Pages/KoiLifespan.jsx';
import Contact from './Pages/Contact.jsx';
import FAQPage from './Pages/FAQ.jsx';
import ForgotPassword from './Pages/ForgotPassword.jsx';
import ResetPassword from './Pages/ResetPassword.jsx';
import Profile from './Pages/Profile.jsx';
import EditProfile from './Pages/EditProfile.jsx';
import Services from './Pages/Services.jsx';
import Pond from './Pages/Pond&Treatment.jsx';
import Consultation from './Pages/Consultation.jsx';
import ProfileDoctor from './Pages/ProfileDoctor.jsx';
import Consultpayment from './Pages/Consultpayment.jsx';
import Payment from './Pages/Tpayment.jsx';
import History from './Pages/History.jsx';




function App() {
  const [count, setCount] = useState(0);
  
  return (
    <Router>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<SignIn />} />
        <Route path="/Sign-Up" element={<SignUp />} />
        <Route path="/StandardCare" element={<StandardCare />} />
        <Route path="/AquaticPlant" element={<AquaticPlant />} />
        <Route path="/KoiFood" element={<KoiFood />} />
        <Route path="/KoiLifespan" element={<KoiLifespan />} />
        <Route path="/Contact" element={<Contact />} />
        <Route path="/FAQPage" element={<FAQPage />} />
        <Route path="/ForgotPassword" element={<ForgotPassword />} />
        <Route path="/ResetPassword" element={<ResetPassword />} />
        <Route path="/Profile" element={<Profile />} />
        <Route path="/edit-profile" element={<EditProfile />} />
        <Route path="/services" element={<Services />} />
        <Route path="/pond" element={<Pond />} />
        <Route path="/Consultation" element={<Consultation />} />
        <Route path="/profile-doctor" element={<ProfileDoctor />} />
        <Route path="/Consultpayment" element={<Consultpayment />} />
        <Route path="/Tpayment" element={<Payment />} />
        <Route path="/History" element={<History />} />

      </Routes>
    </Router>
  );
}

export default App;

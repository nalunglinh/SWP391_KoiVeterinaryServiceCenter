import { useState } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'; // Ensure you import BrowserRouter
import HomePage from './Pages/HomePage';
import SignIn from './Pages/SignIn';
import SignUp from './Pages/SignUp';
import StandardCare from './Pages/StandardCare';
import AquaticPlant from './Pages/AquaticPlant';
import KoiFood from './Pages/KoiFood';
import KoiLifespan from './Pages/KoiLifespan';
import Contact from './Pages/Contact';
import FAQPage from './Pages/FAQ';
import ForgotPassword from './Pages/ForgotPassword';
import ResetPassword from './Pages/ResetPassword';
import Profile from './Pages/Profile';
import EditProfile from './Pages/EditProfile';
import Services from './Pages/Services';
import Pond from './Pages/Pond&Treatment.jsx';
import Consultation from './Pages/Consultation';
import ProfileDoctor from './Pages/ProfileDoctor';
import Consultpayment from './Pages/Consultpayment.jsx';
import Payment from './Pages/Tpayment.jsx';
import History from './Pages/History';




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

import React from "react";
import { Link } from "react-router-dom";
import { logout } from "../modules/authManager";

const Header = ({ isLoggedIn }) => {
  
  return (
    <nav className="navbar navbar-expand navbar-dark bg-info">
      <Link to="/" className="navbar-brand">
        StreamISH
      </Link>
      <ul className="navbar-nav mr-auto">
        {isLoggedIn &&
        <>
          <li className="nav-item">
            <Link to="/" className="nav-link">
              Feed
            </Link>
          </li>
          <li className="nav-item">
            <Link to="/videos/add" className="nav-link">
              New Video
            </Link>
          </li>
          <li className="nav-item">
            <a className="nav-link" onClick={logout}>
              Logout
            </a>
          </li>
        </>
        }
        {!isLoggedIn &&
          <>
            <li className="nav-item">
              <Link to="/login">
                Login
              </Link>
            </li>
            <li className="nav-item">
              <Link to="/register">
                Register
              </Link>
            </li>
          </>
        }
      </ul>
    </nav>
  );
};

export default Header;
import React from "react";
import "./App.css";
import { BrowserRouter as Router } from "react-router-dom";
import ApplicationViews from "./components/ApplicationViews.js";
import Header from "./components/Header.js";

function App() {
  return (
    <div className="App">
      <Router>
        <Header />
        <ApplicationViews />
      </Router>
    </div>
  );
}

export default App;
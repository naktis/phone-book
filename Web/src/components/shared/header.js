import React from "react";
import './shared.css';

function Header(props) {
  return(
    <div className="header">
        <h1>Phone Book</h1>
				<div className="user-data-div">
					{props.children}
				</div>
      </div>
  )
}

export default Header;
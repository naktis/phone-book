import React from "react";
import './shared.css';

function GenericPage(props) {
  return(
    <div className="generic-page">
      <div className="generic-content">
        {props.children}
      </div>
    </div>
  )
}

export default GenericPage;
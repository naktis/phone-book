import React from "react";
import './home.css'

class EntryRow extends React.Component {
  render() {
    return (
        <div className="entry-row">
          <div className="entry-row-data-div">
            <p>{this.props.entry.name}</p>
            {this.props.entry.phone}
          </div>
          <div className="entry-row-button-div">
            <img src="./icons/share.png" alt="share"/>
            <img src="./icons/edit.png" alt="edit"/>
            <img src="./icons/delete.png" alt="delete"/>
          </div>
        </div>
)}
}

export default EntryRow;
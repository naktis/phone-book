import React from "react";
import './home.css'

class UserRow extends React.Component {
  render() {
    return (
      <div className="user-row">
        <img src="./icons/delete.png" alt="Stop sharing" />
        <div className="shared-username">{this.props.user.username}</div>
      </div>
    )
  }
}

export default UserRow;
import React from "react";
import './home.css'
import SuccessMessage from "../shared/messages/successMessage";
import axios from "axios";

class UserRow extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      successMessage: ""
    }
  }
  handleStopSharing() {
    if (!window.confirm(`Are you sure you want to stop sharing ${this.props.entry.name} with user ${this.props.user.username}?`))
      return;

    const _this = this;

    const config = {
			headers: { Authorization: `Bearer ${this.props.owner.token}` }
		}

    console.log(this.props.user);

    axios.delete(`https://localhost:5001/api/Entry/Share/${this.props.entry.entryId}/${this.props.user.userId}`, config).then(function(response) {
      _this.setState({
        successMessage: `Entry sharing has been terminated`
      })
      }).catch((error) => {
        console.log(error);
    })
  }

  render() {
    return (
      <div className="user-row">
        <img 
          src="./icons/delete.png" 
          alt="Stop sharing"
          onClick={this.handleStopSharing.bind(this)}
        />
        <div className="shared-username">{this.props.user.username}</div>
        <SuccessMessage>{this.state.successMessage}</SuccessMessage>
      </div>
    )
  }
}

export default UserRow;
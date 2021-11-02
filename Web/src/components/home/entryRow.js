import React from "react";
import axios from "axios";
import './home.css'
import UserRow from "./userRow";
import SuccessMessage from "../shared/messages/successMessage";
import ValidationError from "../shared/messages/validationError";
import ServerError from "../shared/messages/serverError";
import EditForm from "./editForm";

class EntryRow extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      showShared: false,
      showEditMode: false,
      users: [],
      successMessage: "",
      name: "",
      validationError: "",
      error: "",
      newUserSuccessMessage: "",
      serverErrorMessage: ""
    }
  }

  handleShowUsers() {
    const _this = this;

    const config = {
			headers: { Authorization: `Bearer ${this.props.user.token}` }
		}

		axios.get(`https://localhost:5001/api/User/${this.props.entry.entryId}`, config).then(function(response) {
      _this.setState({
        users: response.data
      })
      }).catch((error) => {
        console.log(error);
    })

    this.setState({
      showShared: !this.state.showShared,
      newUserSuccessMessage: ""
    })
  }

  handleDelete() {
    if (!window.confirm(`Are you sure you want to delete phone number of ${this.props.entry.name}?`))
      return;

    const _this = this;

    const config = {
			headers: { Authorization: `Bearer ${this.props.user.token}` }
		}

    axios.delete(`https://localhost:5001/api/Entry/${this.props.entry.entryId}`, config).then(function(response) {
      _this.setState({
        successMessage: `Phone of ${_this.props.entry.name} has been deleted. Please refresh contacts.`
      })
      }).catch((error) => {
        console.log(error);
    })
  }

  handleNameChange(e) {
		this.setState({
      name: e.target.value,
      duplicateMessage: "",
      successMessage: "",
      newUserSuccessMessage: "",
      serverErrorMessage: ""
    })
  }

  handleValidation() {
    let formValid = true;
    let error = "";
    let name = this.state.name;

		if (!name) {
      formValid = false;
      error = "Enter a user name";
    } else if (name.length < 3) {
      formValid = false;
      error = "User name must consist of at least 3 symbols";
    } else if (!name.match("^[A-Za-z0-9]+$")) {
      formValid = false;
      error = "Name must consist of letters and digits only";
    }

		this.setState({ error: error });
    return formValid;
  }

  collectData(e) {
		e.preventDefault();

		if (!this.handleValidation())
      return;

    const _this = this;

    const config = {
			headers: { Authorization: `Bearer ${this.props.user.token}` }
		}

    axios.post(`https://localhost:5001/api/Entry/Share?entryKey=${this.props.entry.entryId}&receiverUsername=${this.state.name}`, config, config).then(function(response) {
      _this.setState({
        newUserSuccessMessage: `Entry has been shared with ${_this.state.name}`,
        name: ""
      })
      }).catch((error) => {
        _this.setState({
          serverErrorMessage: `User ${_this.state.name} either doesn't exist or already has access to the entry`
        })
        console.log(error);
    })
  }

  render() {
    return (
      <div className="entry-row-container">
        <div className="entry-row">
          <div className="entry-row-data-div">
            <SuccessMessage>{this.state.successMessage}</SuccessMessage>
            <div className="entry-name">{this.props.entry.name}</div>
            {this.props.entry.ownerId !== this.props.user.id ? 
            <div className="shared-text">Shared by {this.props.entry.ownerUsername}</div> : null}
            <a href={"tel:"+this.props.entry.phone}>{this.props.entry.phone}</a>
          </div>
          <div className="entry-row-button-div">
            {this.props.entry.ownerId !== this.props.user.id ? null : 
            <div>
              <img 
                src="./icons/share.png" 
                alt="share" 
                onClick={this.handleShowUsers.bind(this)}
              />
              <img 
                src="./icons/edit.png" 
                alt="edit"
                onClick={() => this.setState({ showEditMode: !this.state.showEditMode })}
              />
              <img 
                src="./icons/delete.png" 
                alt="delete"
                onClick={this.handleDelete.bind(this)}
              />
            </div>
            }
          </div>
        </div>

        {this.state.showShared ? 
          <div>
            <div className="shared-users-header">Share entry with another user</div>
            <div className="share-new-div">
              <label>User name</label>
              <input 
                type="text"
                value={this.state.name}
                onChange={this.handleNameChange.bind(this)}
                maxLength="30"
              />
              <input type="submit" value="SHARE" onClick={ this.collectData.bind(this) }/>
            </div>
            <ServerError>{this.state.serverErrorMessage}</ServerError>
            <SuccessMessage>{this.state.newUserSuccessMessage}</SuccessMessage>
            <ValidationError>{this.state.error}</ValidationError>
            {this.state.users.length === 0 ? 
              <div className="shared-users-header">Entry hasn't been shared with anyone.</div>
              : <div className="shared-users-header">Shared with:</div>}
            <div>{ this.state.users.map(function (user){
              return (<UserRow entry={this.props.entry} key={user.userId} owner={this.props.user} user={user} />
            )}, this) }</div>
          </div>
        : null}

        {this.state.showEditMode ? 
        <div>
          <EditForm user={this.props.user} entry={this.props.entry}></EditForm>
        </div> 
        : null}
      </div>
)}
}

export default EntryRow;
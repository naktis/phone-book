import React from "react";
import axios from "axios";
import './home.css'
import UserRow from "./userRow";

class EntryRow extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      showShared: false,
      users: []
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

    this.setState({showShared: !this.state.showShared})
  }

  render() {
    return (
      <div className="entry-row-container">
        <div className="entry-row">
          <div className="entry-row-data-div">
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
              <img src="./icons/edit.png" alt="edit"/>
              <img src="./icons/delete.png" alt="delete"/>
            </div>
            }
          </div>
        </div>
        {this.state.showShared ? 
          <div>
            {this.state.users.length === 0 ? 
              <div className="shared-users-header">Entry hasn't been shared with anyone.</div>
              : <div className="shared-users-header">Shared with:</div>}
            <div>{ this.state.users.map(function (user){
              return (<UserRow entry={this.props.entry} key={user.userId} owner={this.props.user} user={user} />
            )}, this) }</div>
          </div>
        : null}
      </div>
)}
}

export default EntryRow;
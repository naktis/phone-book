import React from "react";
import GenericPage from "../shared/genericPage";
import axios from "axios";
import { Redirect } from 'react-router-dom';
import EntryRow from "./entryRow";
import './home.css'
import EntryFrom from "./entryForm";
import AccountSettings from "./accountSettings";

class HomePage extends React.Component {
	constructor(props) {
		super(props);

		this.state = {
			entries: [],
			token: "",
			paging: {
        currentPage: 1,
        nextEntries: [],
        buttons: {
          back: {
            state: "disabled",
            class: "disabled-button"
          },
          next: {
            state: "",
            class: ""
          }					
        }
      }
		}
	}

	componentDidMount() {
		const _this = this;

		if(this.props.location.user === undefined)
			return;

		const config = {
			headers: { Authorization: `Bearer ${this.props.location.user.token}` }
		}

		axios.get("https://localhost:5001/api/Entry", config).then(function(response) {
      _this.setState({
        entries: response.data
      })
      }).catch((error) => {
        console.log(error);
    })

    axios.get("https://localhost:5001/api/Entry?PageNumber=2", config).then(function(response) {
      let paging = _this.state.paging;
      paging.nextEntries = response.data
      _this.setState({ paging: paging})
      _this.handleNextButtonEnable();
      }).catch((error) => {
        console.log(error);
    })
	}

	refreshEntries(pageNumber) {
		const _this = this;

		const config = {
			headers: { Authorization: `Bearer ${this.props.location.user.token}` }
		}

		axios.get(`https://localhost:5001/api/Entry?PageNumber=${pageNumber}`, config).then(function(response) {
      _this.setState({
        entries: response.data
      });
      }).catch((error) => {
        console.log(error);
    })
	}

	nextPage() {
    const _this = this;
    let currentPage = this.state.paging.currentPage;

    this.refreshEntries(currentPage+1);

		const config = {
			headers: { Authorization: `Bearer ${this.props.location.user.token}` }
		}

    axios.get(`https://localhost:5001/api/Entry?PageNumber=${currentPage+2}`, config)
      .then(function(response) {
        let paging = _this.state.paging;
        paging.nextEntries = response.data

        if(_this.state.paging.currentPage === 1) {
          paging.buttons.back.state = "";
          paging.buttons.back.class = "";
        }
        paging.currentPage = currentPage+1;

        _this.setState({ paging: paging})
        _this.handleNextButtonEnable();
      }).catch((error) => {
        console.log(error);
    })
  }

	handleNextButtonEnable() {
    let paging = this.state.paging;

    if(this.state.paging.nextEntries.length === 0 ) {
      paging.buttons.next.state = "disabled";
      paging.buttons.next.class = "disabled-button";
    }

    this.setState({ paging: paging })
  }

	previousPage() {
    const _this = this;
    let currentPage = this.state.paging.currentPage;

    this.refreshEntries(currentPage-1);

    let paging = _this.state.paging;
    paging.buttons.next.state = "";
    paging.buttons.next.class = "";
    paging.currentPage = currentPage-1;

    if(paging.currentPage === 1) {
      paging.buttons.back.state = "disabled";
      paging.buttons.back.class = "disabled-button";
    }
    _this.setState({ paging: paging})
  }

	getUser() {
		if (this.props.location.user === undefined)
			return { user: { userId: 0, username: "", email: ""}}

		return this.props.location.user;
	}
	
	render() {
		return(
			<GenericPage>
				{this.props.location.user === undefined ? <Redirect to="/login" /> : <h2>Your contacts</h2>}
				{this.state.entries.length === 0 ? <p>You don't have any contacts yet.</p> :
				<div>
					{ this.state.entries.map(function (entry){
							return (<EntryRow entry={entry} key={entry.entryId} user={this.getUser.bind(this)()} />
					)}, this) }
				
				<div className="paging-div">
          <button 
            disabled={this.state.paging.buttons.back.state}
            className={this.state.paging.buttons.back.class}
            onClick={this.previousPage.bind(this)}>
            &#8592;
          </button>
          <div>{this.state.paging.currentPage}</div>
          <button 
            disabled={this.state.paging.buttons.next.state} 
            className={this.state.paging.buttons.next.class}
            onClick={this.nextPage.bind(this)}>
            &#8594;
          </button>
        </div>
				</div>
				}
				<EntryFrom user={this.props.location.user}/>
				<AccountSettings user={this.props.location.user}/>
			</GenericPage>
		)
	}
}

export default HomePage;
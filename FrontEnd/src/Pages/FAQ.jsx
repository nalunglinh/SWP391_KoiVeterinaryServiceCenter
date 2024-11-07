import React from "react";
import Header from "../components/Header";
import Footer from "../components/Footer";

export default function FAQPage() {
  return (
    <>
      <Header></Header>
      <div className="container my-5">
        <h2 className="faq-header">Fish Care FAQ</h2>
        <div className="accordion" id="faqAccordion">
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingOne">
              <button
                className="accordion-button"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseOne"
                aria-expanded="true"
                aria-controls="collapseOne"
              >
                How many times a day should Koi fish be fed?
              </button>
            </h2>
            <div
              id="collapseOne"
              className="accordion-collapse collapse show"
              aria-labelledby="headingOne"
              data-bs-parent="#faqAccordion"
            >
              <div className="accordion-body">
                Koi should be fed in small amounts within 5-10 minutes to avoid
                excess food contaminating the water. The frequency and amount of
                feeding depends on the age, size, temperature, and type of food.
                It is important to ensure that the food contains adequate
                nutrients such as protein, fat, carbohydrates, vitamins, and
                minerals. Koi do not have stomachs and cannot store food, so it
                is ideal to feed small amounts several times a day. Adjusting
                the portion size based on the temperature and stage of growth of
                the fish is necessary.
              </div>
            </div>
          </div>
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingTwo">
              <button
                className="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseTwo"
                aria-expanded="false"
                aria-controls="collapseTwo"
              >
                What is the appropriate density for Koi fish stocking?
              </button>
            </h2>
            <div
              id="collapseTwo"
              className="accordion-collapse collapse"
              aria-labelledby="headingTwo"
              data-bs-parent="#faqAccordion"
            >
              <div className="accordion-body">
                When building an outdoor aquarium and designing a Koi pond
                filter, it is necessary to take into account the number of fish
                and the size of the fish to be released. According to the
                principle, for Koi fish larger than 30 cm, one fish can be
                released for every cubic meter of water. For small fish, we can
                release them at a higher density.
              </div>
            </div>
          </div>
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingThree">
              <button
                className="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseThree"
                aria-expanded="false"
                aria-controls="collapseThree"
              >
                Do I need to change the water in a Koi pond regularly?
              </button>
            </h2>
            <div
              id="collapseThree"
              className="accordion-collapse collapse"
              aria-labelledby="headingThree"
              data-bs-parent="#faqAccordion"
            >
              <div className="accordion-body">
                If the Koi pond is less than 10m3, change 30% of the water every
                month. The larger the pond, the less water needs to be changed.
                Some large Koi ponds can use water to water plants or combine
                with a very flexible and effective fire protection function.
              </div>
            </div>
          </div>
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingFour">
              <button
                className="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseFour"
                aria-expanded="false"
                aria-controls="collapseFour"
              >
                Which pond is better? Rubber pond or concrete pond?
              </button>
            </h2>
            <div
              id="collapseFour"
              className="accordion-collapse collapse"
              aria-labelledby="headingFour"
              data-bs-parent="#faqAccordion"
            >
              <div className="accordion-body">
                It is advisable to build a pond with concrete because of its
                stable bearing capacity over many years. A dark pond floor is
                better. A dark pond floor, for example, black or dark blue, will
                help us to admire the splendid beauty of the Koi fish best. And
                especially limit the stains on the pond wall due to moss that
                has accumulated over time.
              </div>
            </div>
          </div>
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingFive">
              <button
                className="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseFive"
                aria-expanded="false"
                aria-controls="collapseFive"
              >
                How to detect sick Koi fish?
              </button>
            </h2>
            <div
              id="collapseFive"
              className="accordion-collapse collapse"
              aria-labelledby="headingFive"
              data-bs-parent="#faqAccordion"
            >
              <div className="accordion-body">
                Observe fish for unusual signs such as slow swimming, not
                eating, floating lethargically on the water surface, ulcers, or
                white spots on the skin.
              </div>
            </div>
          </div>
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingSix">
              <button
                className="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseSix"
                aria-expanded="false"
                aria-controls="collapseSix"
              >
                What are common diseases in Koi fish and how to prevent them?
              </button>
            </h2>
            <div
              id="collapseSix"
              className="accordion-collapse collapse"
              aria-labelledby="headingSix"
              data-bs-parent="#faqAccordion"
            >
              <div className="accordion-body">
                Fungal skin diseases, ulcers, parasites and infections. Prevent
                by maintaining clean water, stable temperature and regular
                health checks for fish.
              </div>
            </div>
          </div>
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingSeven">
              <button
                className="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseSeven"
                aria-expanded="false"
                aria-controls="collapseSeven"
              >
                Koi fish have skin fungus, how to treat it?
              </button>
            </h2>
            <div
              id="collapseSeven"
              className="accordion-collapse collapse"
              aria-labelledby="headingSeven"
              data-bs-parent="#faqAccordion"
            >
              <div className="accordion-body">
                Separate diseased fish, use salt or a specific antifungal
                medication, and make sure to improve water quality to limit
                fungal growth.
              </div>
            </div>
          </div>
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingEight">
              <button
                className="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseEight"
                aria-expanded="false"
                aria-controls="collapseEight"
              >
                Should sick Koi fish be isolated from the common pond?
              </button>
            </h2>
            <div
              id="collapseEight"
              className="accordion-collapse collapse"
              aria-labelledby="headingEight"
              data-bs-parent="#faqAccordion"
            >
              <div className="accordion-body">
                Sick fish should be isolated to prevent spread and to easily
                monitor and treat fish in a controlled environment.
              </div>
            </div>
          </div>
        </div>
      </div>

      <Footer />
    </>
  );
}
